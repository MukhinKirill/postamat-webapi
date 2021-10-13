using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasData(
                new Order
                {
                    Number = 1,
                    Cost = new decimal(1000.21),
                    FullName = "Ivanov Ivan Ivanovich",
                    PhoneNumber = "+7999-111-22-33",
                    PostamatId = 1,
                    Status = OrderStatus.Registered
                },
                new Order
                {
                    Number = 2,
                    Cost = new decimal(2000.21),
                    FullName = "Denisov Ivan Ivanovich",
                    PhoneNumber = "+7999-222-22-33",
                    PostamatId = 2,
                    Status = OrderStatus.InStock
                }
            );
        }
    }
}