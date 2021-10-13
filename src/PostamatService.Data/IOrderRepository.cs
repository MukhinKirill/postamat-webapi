using PostamatService.Data.Models;

namespace PostamatService.Data
{
    public interface IOrderRepository
    {
        void Create(Order order);
        void Update(Order order);
        Order Get(int number);
    }
}
