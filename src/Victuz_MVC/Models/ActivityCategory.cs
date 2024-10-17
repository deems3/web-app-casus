using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class ActivityCategory
    {
        public int Id { get; set; }


        [Required]
        [DataType (DataType.Text)]
        public string Name { get; set; } = null!;
    }
}
