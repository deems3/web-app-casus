using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? Category { get; set; }
        public int Limit { get; set; }
        public DateTime DateTime { get; set; }


        public ICollection<Account>? Hosts { get; set; }

    }
}
