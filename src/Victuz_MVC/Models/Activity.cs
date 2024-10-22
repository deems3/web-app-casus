using System.ComponentModel.DataAnnotations;
using Victuz_MVC.Enums;

namespace Victuz_MVC.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht.")]
        [StringLength(25, ErrorMessage = "Maximaal 25 karakters toegestaan.")]
        public string? Name { get; set; }

        [StringLength(255, ErrorMessage = "Maximaal 255 karakters toegestaan.")]
        public string? Description { get; set; }

        public int Limit { get; set; }

        public DateTime DateTime { get; set; }

        // Responsible people
        public ICollection<Account> Hosts { get; } = [];

        // Status enum
        public ActivityStatus Status { get; set; } = ActivityStatus.Processing;

        public int ActivityCategoryId { get; set; }
        public ActivityCategory? ActivityCategory { get; set; } = null!; // TODO: temporarily made ActivityCategory nullable to bypass FormState validation

        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }
    }
}
