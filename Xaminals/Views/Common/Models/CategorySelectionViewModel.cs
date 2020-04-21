using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Offers.Models;

namespace exchaup.Views.Common.Models
{
    public partial class CategorySelectionViewModel : BaseViewModel
    {
        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> FilteredCategories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        private ObservableCollection<CategoryModel> _selectedCategories;
        public ObservableCollection<CategoryModel> SelectedCategories
        {
            get { return _selectedCategories; }
            set { _selectedCategories = value; }
        }

        private List<CategoryModel> _availableCategories;
        public List<CategoryModel> AvailableCategories
        {
            get { return _availableCategories; }
            set { _availableCategories = value; }
        }

        private string _query;
        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }

        private bool _MultiSelection = true;
        public bool MultiSelection
        {
            get { return _MultiSelection; }
            set { SetProperty(ref _MultiSelection, value); }
        }

        private ICategorySelectable _SelectInto;
        public ICategorySelectable SelectInto
        {
            get { return _SelectInto; }
            set { SetProperty(ref _SelectInto, value); }
        }


        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _availableCategories = new List<CategoryModel>();
            _categories = new ObservableCollection<CategoryModel>();
            _selectedCategories = new ObservableCollection<CategoryModel>();
        }
    }
}