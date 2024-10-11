using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
