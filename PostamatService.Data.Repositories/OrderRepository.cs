using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories
{
    public sealed class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
        public void CreateOrder(Order order) => Create(order);
        public void UpdateOrder(Order order) => Update(order);
        public async Task<Order> Get(int number, bool trackChanges) =>
            await FindByCondition(_ => _.Number == number, trackChanges)
                .SingleOrDefaultAsync();

        public async Task SaveAsync() => await RepositoryContext.SaveChangesAsync();
    }
}
