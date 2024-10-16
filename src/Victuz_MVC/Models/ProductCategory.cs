using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; } = null!;

        public List<Product>? Products { get; set; }
    }
}
