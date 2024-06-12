using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechAssessment_C_.Models
{
    public class Products
    {
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public float? Price { get; set; }
    }
}
