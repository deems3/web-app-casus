using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht.")]
        [StringLength(25, ErrorMessage = "Maximaal 25 karakters toegestaan.")]
        public string? Name { get; set; }

        public string? Description { get; set; }
        public int Limit { get; set; }
        public DateTime DateTime { get; set; }


        public ICollection<Account>? Hosts { get; set; }
       

        //catergory connectie
        public int CategoryId { get; set; }
        public ActivityCategory Category { get; set; } = null!;

    }
}
