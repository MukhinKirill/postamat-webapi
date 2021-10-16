using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PostamatService.Data.Models
{
    public class Order
    {
        public Order(OrderStatus status, int postamatId)
        {
            Status = status;
            PostamatId = postamatId;
            Products = new List<ProductInOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }
        [Required]
        public OrderStatus Status { get; private set; }
        [Required]
        public ICollection<ProductInOrder> Products { get; }
        [Required]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Client phone is a required field.")]
        [StringLength(15, ErrorMessage = "Length for the PhoneNumber is 15 characters.", MinimumLength = 15)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Client fullname name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the FullName is 100 characters.")]
        public string FullName { get; set; }
        [ForeignKey(nameof(Postamat))]
        public int PostamatId { get; private set; }
        public Postamat Postamat { get; set; }
        public string PostamatNumber => Postamat.Number;

        public void UpdateProducts(IEnumerable<string> products)
        {
            foreach (var product in products.Except(Products.Select(_ => _.Name)))
            {
                Products.Add(new ProductInOrder { Name = product, OrderId = Number });
            }

            var removesProducts = Products.Select(_ => _.Name).Except(products).ToList();
            foreach (var product in removesProducts)
            {
                var removeProduct = Products.Single(_ => _.Name == product);
                Products.Remove(removeProduct);
            }
        }

        public void Cancel() => Status = OrderStatus.Canceled;
    }
}
