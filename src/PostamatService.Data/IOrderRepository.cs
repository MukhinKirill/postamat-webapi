using System.Threading.Tasks;
using PostamatService.Data.Models;

namespace PostamatService.Data
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        Task<Order> Get(int number, bool trackChanges);
    }
}
