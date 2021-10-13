using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories.Configurations
{
    public class PostamatConfiguration : IEntityTypeConfiguration<Postamat>
    {
        public void Configure(EntityTypeBuilder<Postamat> builder)
        {
            builder.HasIndex(_ => _.Number);
            builder.HasData(
                new Postamat
                {
                    Id = 1,
                    Number = "1111-222",
                    IsActive = true,
                    Address = "Красная пл., 3, Москва, 109012"
                },
                new Postamat
                {
                    Id = 2,
                    Number = "1111-111",
                    IsActive = true,
                    Address = "ул. Энгельса, 18, Большие Дворы, Московская обл., 142541"
                },
                new Postamat
                {
                    Id = 3,
                    Number = "1111-000",
                    IsActive = false,
                    Address = "Дворцовая пл., 2, Санкт-Петербург, 190000"
                });
        }
    }
}