namespace Victuz_MVC.Models
{
    public class AccountActivity
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }


        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }


    }
}
