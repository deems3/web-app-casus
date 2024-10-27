namespace Victuz_MVC.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public DateTime EnrolledAt { get; set; }

        // Account is a GUID in the database and this is converted into a string
        public string AccountId { get; set; } = null!;
        public Account Account { get; set; } = null!;

        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
    }
}
