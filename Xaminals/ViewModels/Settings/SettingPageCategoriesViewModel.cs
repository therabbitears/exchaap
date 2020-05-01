using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.ViewModels.Settings
{
    public partial class SettingPageCategoriesViewModel : BaseViewModel
    {
        ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Categories = new ObservableCollection<CategoryModel>();
        }
    }
}
