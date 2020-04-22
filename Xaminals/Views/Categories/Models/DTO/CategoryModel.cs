using Xaminals.Models;

namespace Xaminals.Views.Categories.Models.DTO
{
    public class CategoryModel : NotificableObject
    {
        public string Id { get; set; }
        
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value;OnPropertyChanged("Name"); }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("Selected"); }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        public string ParentId { get; set; }
        public bool IsParent { get { return string.IsNullOrEmpty(ParentId); } }
    }
}
