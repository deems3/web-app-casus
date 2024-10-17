using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class ProductCategoryLine
    {
        [Key]
        public int Id { get; set; }


        public int ProductId { get; set; }
        public Product? Product { get; set; }


        public int ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
    }
}
