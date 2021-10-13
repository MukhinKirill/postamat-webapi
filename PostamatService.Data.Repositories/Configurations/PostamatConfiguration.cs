using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories.Configurations
{
    public class PostamatConfiguration : IEntityTypeConfiguration<Postamat>
    {
        public void Configure(EntityTypeBuilder<Postamat> builder)
        {
            builder.HasData(
                new Postamat
                {
                    Number = "1111-222",
                    IsActive = true,
                    Address = "Красная пл., 3, Москва, 109012"
                },
                new Postamat
                {
                    Number = "1111-111",
                    IsActive = true,
                    Address = "ул. Энгельса, 18, Большие Дворы, Московская обл., 142541"
                },
                new Postamat
                {
                    Number = "1111-000",
                    IsActive = false,
                    Address = "Дворцовая пл., 2, Санкт-Петербург, 190000"
                });
        }
    }
}