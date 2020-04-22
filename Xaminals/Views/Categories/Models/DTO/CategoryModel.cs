using Xaminals.Models;

namespace Xaminals.Views.Categories.Models.DTO
{
    public class CategoryModel : NotificableObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("Selected"); }
        }

        public string Image { get; set; }
        public string ParentId { get; set; }
        public bool IsParent { get { return string.IsNullOrEmpty(ParentId); } }
    }
}
