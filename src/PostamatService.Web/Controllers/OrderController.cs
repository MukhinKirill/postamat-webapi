using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PostamatService.Data;
using PostamatService.Data.Models;
using PostamatService.Web.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PostamatService.Web.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPostamatRepository _postamatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderRepository orderRepository, IPostamatRepository postamatRepository, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _postamatRepository = postamatRepository;
            _mapper = mapper;
            _logger = logger;
        }
        // GET api/<OrderController>/5
        [HttpGet("{number}", Name = "GetOrder")]
        public async Task<IActionResult> Get(int number)
        {
            var order = await _orderRepository.Get(number, false);
            if (order is null)
            {
                _logger.LogInformation($"Order with number: {number} doesn't exist.");
                return NotFound();
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderForCreateDto order)
        {
            if (order.Products.Length >= 10
                || !Regex.IsMatch(order.PostamatNumber, @"^\d{4}-\d{3}$")
                || !Regex.IsMatch(order.PhoneNumber, @"^\+7\d{3}-\d{3}-\d{2}-\d{2}$")
                )
            {
                return BadRequest();
            }

            var postamat = await _postamatRepository.Get(order.PostamatNumber, false);
            if (postamat is null)
            {
                return BadRequest();
            }

            if (!postamat.IsActive)
            {
                return Forbid();
            }

            var orderEntity = _mapper.Map<Order>(order);
            orderEntity.PostamatId = postamat.Id;
            _orderRepository.CreateOrder(orderEntity);
            await _orderRepository.SaveAsync();
            orderEntity.Products = order.Products.Select(_ => new ProductInOrder
            {
                Name = _,
                OrderId = orderEntity.Number
            }).ToList();
            await _orderRepository.SaveAsync();
            orderEntity.Postamat = postamat;
            var orderToReturn = _mapper.Map<OrderDto>(orderEntity);
            return CreatedAtRoute("GetOrder", new
            {
                number = orderToReturn.Number
            }, orderToReturn);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{number}")]
        public async Task<IActionResult> Put(int number, [FromBody] OrderForUpdateDto order)
        {
            var orderEntity = await _orderRepository.Get(number, true);
            if (orderEntity is null)
            {
                _logger.LogInformation($"Order with number: {number} doesn't exist.");
                return NotFound();
            }
            if (order.Products.Length >= 10
                || !Regex.IsMatch(order.PhoneNumber, @"^\+7\d{3}-\d{3}-\d{2}-\d{2}$"))
            {
                return BadRequest();
            }

            _mapper.Map(order, orderEntity);
            foreach (var product in order.Products.Except(orderEntity.Products.Select(_ => _.Name)))
            {
                orderEntity.Products.Add(new ProductInOrder { Name = product, OrderId = orderEntity.Number });
            }
            foreach (var product in orderEntity.Products.Select(_ => _.Name).Except(order.Products))
            {
                var removeProduct = orderEntity.Products.Single(_ => _.Name == product);
                orderEntity.Products.Remove(removeProduct);
            }
            _orderRepository.UpdateOrder(orderEntity);
            await _orderRepository.SaveAsync();
            orderEntity.Products = order.Products.Select(_ => new ProductInOrder
            {
                Name = _,
                OrderId = orderEntity.Number
            }).ToList();
            await _orderRepository.SaveAsync();
            return NoContent();
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{number}")]
        public async Task<IActionResult> Delete(int number)
        {
            var orderEntity = await _orderRepository.Get(number, true);
            if (orderEntity is null)
            {
                _logger.LogInformation($"Order with number: {number} doesn't exist.");
                return NotFound();
            }

            orderEntity.Status = OrderStatus.Canceled;
            _orderRepository.UpdateOrder(orderEntity);
            await _orderRepository.SaveAsync();
            return NoContent();
        }
    }
}
