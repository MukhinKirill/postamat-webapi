using System.ComponentModel.DataAnnotations;

namespace PostamatService.Web.DTO
{
    public abstract class OrderForManipulationDto
    {
        public string[] Products { get; set; }
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Client phone is a required field.")]
        [StringLength(15, ErrorMessage = "Length for the PhoneNumber is 15 characters.", MinimumLength = 15)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Client name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the FullName is 100 characters.")]
        public string FullName { get; set; }
    }
}
