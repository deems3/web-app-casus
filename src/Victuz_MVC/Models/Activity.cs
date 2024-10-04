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

        public string? BestuurslidId { get; set; }
        public Bestuurslid? Bestuurslid { get; set; }

        public List<Lid>? ResponsibleStudentsIds { get; set; }

    }
}
