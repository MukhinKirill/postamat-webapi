using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PostamatService.Data;

namespace PostamatService.Web.ActionFilters
{
    public class ValidateOrderExistAttribute : IAsyncActionFilter
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<ValidateOrderExistAttribute> _logger;

        public ValidateOrderExistAttribute(IOrderRepository orderRepository, ILogger<ValidateOrderExistAttribute> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var number = (int)context.ActionArguments["number"];
            var order = await _orderRepository.Get(number, trackChanges);
            if (order == null)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("order", order);
                await next();
            }
        }
    }
}
