using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories.Configurations
{
    public class ProductInOrderConfiguration : IEntityTypeConfiguration<ProductInOrder>
    {
        public void Configure(EntityTypeBuilder<ProductInOrder> builder)
        {
            builder.HasData(
                new ProductInOrder
                {
                    Id = 1,
                    Name = "Sony s1",
                    OrderId = 1
                },
                new ProductInOrder
                {
                    Id = 2,
                    Name = "Pony p1",
                    OrderId = 1
                },
                new ProductInOrder
                {
                    Id = 3,
                    Name = "Johny j1",
                    OrderId = 1
                },
                new ProductInOrder
                {
                    Id = 4,
                    Name = "Sony s2",
                    OrderId = 2
                },
                new ProductInOrder
                {
                    Id = 5,
                    Name = "Pony p2",
                    OrderId = 2
                },
                new ProductInOrder
                {
                    Id = 6,
                    Name = "Johny j2",
                    OrderId = 2
                });
        }
    }
}
