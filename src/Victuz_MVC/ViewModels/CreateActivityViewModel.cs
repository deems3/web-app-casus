using Victuz_MVC.Models;

namespace Victuz_MVC.ViewModels
{
    public class CreateActivityViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Limit { get; set; }
        public DateTime DateTime { get; set; }
        public int ActivityCategoryId { get; set; }
        public ICollection<Account>? Hosts { get; set; }
    }
}
