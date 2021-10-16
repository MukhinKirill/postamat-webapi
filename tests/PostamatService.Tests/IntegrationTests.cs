using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PostamatService.Data.Models;
using PostamatService.Web;
using PostamatService.Web.DTO;
using Xunit;

namespace PostamatService.Tests
{
    public class IntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetActivePostamats()
        {
            var response = await _client.GetAsync("api/postamat/active");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var postamats = JsonConvert.DeserializeObject<IEnumerable<PostamatDto>>(responseString);
            Assert.NotNull(postamats);
            Assert.NotEmpty(postamats);
            Assert.Equal(2, postamats.Count());
        }

        [Fact]
        public async Task GetPostamat()
        {
            var response = await _client.GetAsync("api/postamat/1111-000");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var postamat = JsonConvert.DeserializeObject<PostamatDto>(responseString);
            Assert.NotNull(postamat);
            Assert.False(postamat.IsActive);
        }

        [Fact]
        public async Task GetNotFoundPostamat()
        {
            var response = await _client.GetAsync("api/postamat/1112-000");

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetOrder()
        {
            var response = await _client.GetAsync($"api/order/1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderDto>(responseString);
            Assert.NotNull(order);
        }

        [Fact]
        public async Task GetOrderNotFound()
        {
            var response = await _client.GetAsync($"api/order/11111111");

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task DeleteOrderNotFound()
        {
            var response = await _client.DeleteAsync($"api/order/11111111");

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task DeleteOrder()
        {
            var responseDelete = await _client.DeleteAsync($"api/order/2");
            responseDelete.EnsureSuccessStatusCode();
            var responseGet = await _client.GetAsync($"api/order/2");
            responseGet.EnsureSuccessStatusCode();
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderDto>(responseString);
            Assert.NotNull(order);
            Assert.Equal(OrderStatus.Canceled,order.Status);
        }

        [Fact]
        public async Task PutOrderNotFound()
        {
            var order = new OrderForUpdateDto()
            {
                Cost = new decimal(1000.21),
                FullName = "Ivanov Ivan Ivanovich",
                PhoneNumber = "+7999-111-22-33",
                Products = new[] { "product" }
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PutAsync($"api/order/11111111",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PutOrderProductsMoreThanTen()
        {
            var order = new OrderForUpdateDto()
            {
                Cost = new decimal(1000.21),
                FullName = "Ivanov Ivan Ivanovich",
                PhoneNumber = "+7999-111-22-33",
                Products = new[]
                {
                    "product0",
                    "product1",
                    "product2",
                    "product3",
                    "product4",
                    "product5",
                    "product6",
                    "product7",
                    "product8",
                    "product9",
                    "product10",
                }
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PutAsync($"api/order/1",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PutOrder()
        {
            var order = new OrderForUpdateDto()
            {
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] { "product" }
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var responsePut = await _client.PutAsync($"api/order/1",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            responsePut.EnsureSuccessStatusCode();
            var responseGet = await _client.GetAsync($"api/order/1");

            var responseString = await responseGet.Content.ReadAsStringAsync();
            var updatedOrder = JsonConvert.DeserializeObject<OrderDto>(responseString);
            Assert.NotNull(updatedOrder);
            Assert.Equal(order.Cost, updatedOrder.Cost);
            Assert.Equal(order.FullName, updatedOrder.FullName);
            Assert.Equal(order.PhoneNumber, updatedOrder.PhoneNumber);
            Assert.Equal(order.Products, updatedOrder.Products);
        }

        [Fact]
        public async Task PutOrderTryChangePostamatNumber()
        {
            var order = new OrderForCreateDto()
            {
                Cost = new decimal(23452.21),
                FullName = "Ivanov Filipp Denisovich",
                PhoneNumber = "+7999-131-25-33",
                Products = new[] { "product", "product1" },
                PostamatNumber = "1111-111"
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var responsePut = await _client.PutAsync($"api/order/1",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            responsePut.EnsureSuccessStatusCode();
            var responseGet = await _client.GetAsync($"api/order/1");

            var responseString = await responseGet.Content.ReadAsStringAsync();
            var updatedOrder = JsonConvert.DeserializeObject<OrderDto>(responseString);
            Assert.NotNull(updatedOrder);
            Assert.Equal(order.Cost, updatedOrder.Cost);
            Assert.Equal(order.FullName, updatedOrder.FullName);
            Assert.Equal(order.PhoneNumber, updatedOrder.PhoneNumber);
            Assert.Equal(order.Products, updatedOrder.Products);
            Assert.NotEqual(order.PostamatNumber, updatedOrder.PostamatNumber);
        }
        [Theory]
        [InlineData("89991112233")]
        [InlineData("9991112233")]
        [InlineData("+79991112233")]
        [InlineData("+7(999)111-22-33")]
        [InlineData("8(999)111-22-33")]
        public async Task PutOrderUncorrectPhoneNumber(string phoneNumber)
        {
            var order = new OrderForUpdateDto
            {
                Cost = new decimal(232.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = phoneNumber,
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PostOrder()
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = "1111-222",
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var updatedOrder = JsonConvert.DeserializeObject<OrderDto>(responseString);
            Assert.NotNull(updatedOrder);
            Assert.Equal(order.Cost, updatedOrder.Cost);
            Assert.Equal(order.FullName, updatedOrder.FullName);
            Assert.Equal(order.PhoneNumber, updatedOrder.PhoneNumber);
            Assert.Equal(order.Products, updatedOrder.Products);
            Assert.Equal(order.PostamatNumber, updatedOrder.PostamatNumber);
            Assert.Equal(OrderStatus.Registered, updatedOrder.Status);
        }

        [Theory]
        [InlineData("1111-err")]
        [InlineData("11112-333")]
        [InlineData("1111333")]
        [InlineData("dfs43f")]
        [InlineData("")]
        public async Task PostOrderUncorrectPostamatNumber(string postamatNumber)
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = postamatNumber,
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Theory]
        [InlineData("89991112233")]
        [InlineData("9991112233")]
        [InlineData("+79991112233")]
        [InlineData("+7(999)111-22-33")]
        [InlineData("8(999)111-22-33")]
        public async Task PostOrderUncorrectPhoneNumber(string phoneNumber)
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = "1111-222",
                Cost = new decimal(232.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = phoneNumber,
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PostOrderNotExistPostamat()
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = "9999-879",
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PostOrderProductsMoreThanTen()
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = "1111-222",
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] {
                    "product0",
                    "product1",
                    "product2",
                    "product3",
                    "product4",
                    "product5",
                    "product6",
                    "product7",
                    "product8",
                    "product9",
                    "product10",
                },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PostOrderNotActivePostamat()
        {
            var order = new OrderForCreateDto
            {
                PostamatNumber = "1111-000",
                Cost = new decimal(23432.21),
                FullName = "Ivanov Filipp Ivanovich",
                PhoneNumber = "+7999-131-21-33",
                Products = new[] { "product" },
            };
            var orderJson = JsonConvert.SerializeObject(order);
            var response = await _client.PostAsync($"api/order",
                new StringContent(orderJson, Encoding.UTF8, "application/json"));

            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.Forbidden, statusCode);
        }
    }
}
