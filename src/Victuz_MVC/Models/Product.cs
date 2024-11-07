using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }

        public ICollection<ProductCategoryLine> ProductCategoryLines { get; set; } = [];
    }
}
