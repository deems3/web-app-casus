using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
