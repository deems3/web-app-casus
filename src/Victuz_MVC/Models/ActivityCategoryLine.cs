using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class ActivityCategoryLine
    {
        public int Id { get; set; }

        //activity relation
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        //category relation
        public int ActivityCategoryId { get; set; }
        public ActivityCategory ActivityCategory { get; set; } = null!;

    }
}
