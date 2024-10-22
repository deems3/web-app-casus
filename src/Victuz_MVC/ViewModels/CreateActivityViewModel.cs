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
        public List<Account>? Hosts { get; set; }
        public List<string>? HostIds { get; set; }

        public int? PictureId { get; set; }
        public Picture? Picture { get; set; } 
    }
}
