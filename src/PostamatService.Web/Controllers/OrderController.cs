using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PostamatService.Data;
using PostamatService.Data.Models;
using PostamatService.Web.ActionFilters;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePostamatAttribute))]
        public async Task<IActionResult> Post([FromBody] OrderForCreateDto order)
        {
            var postamat = HttpContext.Items["postamat"] as Postamat;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Put(int number, [FromBody] OrderForUpdateDto order)
        {
            var orderEntity = await _orderRepository.Get(number, true);
            if (orderEntity is null)
            {
                _logger.LogInformation($"Order with number: {number} doesn't exist.");
                return NotFound();
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
            var orderEntity = await _orderRepository.Get(number, true);//todo; add validate of get order
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
