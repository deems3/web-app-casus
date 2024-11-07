using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Victuz_MVC.Enums;

namespace Victuz_MVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } = [];

        [NotMapped]
        public decimal TotalPrice => OrderProducts.Sum(op => op.ProductAmount * op.Product.Price);

        [Required]
        public string AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public DateTime? CompletedAt { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public OrderStatus Status { get; set; } = OrderStatus.Initialized;
    }
}
