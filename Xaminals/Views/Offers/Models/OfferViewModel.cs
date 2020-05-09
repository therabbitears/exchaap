using Plugin.Media.Abstractions;
using Plugin.ValidationRules;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.Models
{
    [QueryProperty("Id", "offerid")]
    public partial class OfferViewModel : BaseViewModel, ICategoriesSelectable, ICategorySelectable
    {
        private ValidatableObject<string> _heading;
        public ValidatableObject<string> Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }


        private ValidatableObject<string> _detail;
        public ValidatableObject<string> Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }


        private string _terms;
        public string Terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        private bool _IsCategorySelected = true;
        public bool IsCategorySelected
        {
            get { return _IsCategorySelected; }
            set { SetProperty(ref _IsCategorySelected, value); }
        }

        private ValidatableObject<DateTime?> _validfrom;
        public ValidatableObject<DateTime?> Validfrom
        {
            get { return _validfrom; }
            set { _validfrom = value; }
        }

        private bool _immedietlyAvailable = true;
        public bool ImmediatelyAvailable
        {
            get { return _immedietlyAvailable; }
            set { SetProperty(ref _immedietlyAvailable, value); }
        }

        private Stream _imageStream;
        public Stream ImageStream
        {
            get { return _imageStream; }
            set
            {
                _imageStream = value;
                OnPropertyChanged("ImageStream");
            }
        }

        private ImageSource imageSource;
        public ImageSource SourceImage
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged("SourceImage"); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _id)
                {
                    _id = newValue;
                    OnPropertyChanged("Id");
                }
            }
        }

        private MediaFile _mediaFile;

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _heading = new ValidatableObject<string>();
            _detail = new ValidatableObject<string>();
            _validfrom = new ValidatableObject<DateTime?>();
            _categories = new ObservableCollection<CategoryModel>();
            _location = new OfferPublisherLocationModel();
            Category = new CategoryModel();
        }

        #region Lists

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

        private CategoryModel categoryModel;
        public CategoryModel Category
        {
            get { return categoryModel; }
            set { SetProperty(ref categoryModel, value); }
        }


        OfferPublisherLocationModel _location;
        public OfferPublisherLocationModel Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public int MaxAllowed => 3;

        #endregion
    }
}