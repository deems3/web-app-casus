using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
