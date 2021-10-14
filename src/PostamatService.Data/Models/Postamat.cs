using System.ComponentModel.DataAnnotations;

namespace PostamatService.Data.Models
{
    public class Postamat
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [StringLength(8, ErrorMessage = "Length for the Number is 8 characters.", MinimumLength = 8)]
        public string Number { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for the Address is 256 characters.")]
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
