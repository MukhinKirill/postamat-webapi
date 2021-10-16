using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PostamatService.Data;
using PostamatService.Web.DTO;

namespace PostamatService.Web.ActionFilters
{
    public class ValidatePostamatAttribute : IAsyncActionFilter
    {
        private readonly IPostamatRepository _postamatRepository;

        public ValidatePostamatAttribute(IPostamatRepository postamatRepository)
        {
            _postamatRepository = postamatRepository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var order = context.ActionArguments.Values.SingleOrDefault() as OrderForCreateDto;
            var postamat = await _postamatRepository.Get(order.PostamatNumber);
            if (postamat is null)
            {
                context.Result = new NotFoundObjectResult($"Postamat with number {order.PostamatNumber} is not exist");
                return;
            }
            if (!postamat.IsActive)
            {
                context.Result = new ForbidResult("custom");
                return;
            }
            context.HttpContext.Items.Add("postamat", postamat);
            await next();
        }
    }
}
