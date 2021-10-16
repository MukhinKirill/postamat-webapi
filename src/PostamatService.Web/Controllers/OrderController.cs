using Microsoft.AspNetCore.Mvc;
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
            var orderEntity = new Order(OrderStatus.Registered, postamat.Id);
            _mapper.Map(order, orderEntity);
            orderEntity.UpdateProducts(order.Products);
            _orderRepository.CreateOrder(orderEntity);
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
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> Put(int number, [FromBody] OrderForUpdateDto order)
        {
            var orderEntity = HttpContext.Items["order"] as Order;
            _mapper.Map(order, orderEntity);
            orderEntity.UpdateProducts(order.Products);
            _orderRepository.UpdateOrder(orderEntity);
            return NoContent();
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{number}")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> Delete(int number)
        {
            var orderEntity = HttpContext.Items["order"] as Order;
            orderEntity.Cancel();
            _orderRepository.UpdateOrder(orderEntity);
            return NoContent();
        }
    }
}
