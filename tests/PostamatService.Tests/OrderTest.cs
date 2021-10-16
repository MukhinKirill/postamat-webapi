using System.Linq;
using PostamatService.Data.Models;
using Xunit;

namespace PostamatService.Tests
{
    public class OrderTest
    {
        [Fact]
        public void UpdateProducts()
        {
            var order = new Order(OrderStatus.DeliveredToPostamat,1);
            var productsForUpdate = new string[]
            {
                "product1",
                "product2",
                "product3"
            };
            order.UpdateProducts(productsForUpdate);
            Assert.Equal(productsForUpdate,order.Products.Select(_ => _.Name));
        }

        [Fact]
        public void UpdateProductsIfAlreadyFill()
        {
            var order = new Order(OrderStatus.DeliveredToPostamat, 1);
            order.Products.Add(new ProductInOrder
            {
                Name = "product1"
            });
            order.Products.Add(new ProductInOrder
            {
                Name = "product4"
            });

            var productsForUpdate = new string[]
            {
                "product1",
                "product2",
                "product3"
            };
            order.UpdateProducts(productsForUpdate);
            Assert.Equal(productsForUpdate, order.Products.Select(_ => _.Name));
        }

        [Fact]
        public void Cancel()
        {
            var order = new Order(OrderStatus.Registered, 1);
            order.Cancel();
            Assert.Equal(OrderStatus.Canceled,order.Status);
        }
    }
}
