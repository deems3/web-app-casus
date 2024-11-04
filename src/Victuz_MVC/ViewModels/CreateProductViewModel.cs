using Victuz_MVC.Models;

namespace Victuz_MVC.ViewModels
{
    public class CreateProductViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? PictureId { get; set; }
        public Picture? Picture {  get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    }
}
