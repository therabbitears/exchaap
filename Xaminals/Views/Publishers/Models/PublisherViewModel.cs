using Plugin.Media.Abstractions;
using Plugin.ValidationRules;
using System;
using System.IO;
using Xamarin.Forms;
using Xaminals.ViewModels;

namespace Xaminals.Views.Publishers.Models
{
    [QueryProperty("Id", "publisherid")]
    public partial class PublisherViewModel : BaseViewModel
    {
        private ValidatableObject<string> _name;
        public ValidatableObject<string> Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private ValidatableObject<string> _description;
        public ValidatableObject<string> Description
        {
            get { return _description; }
            set { _description = value; }
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

        
        public string Image { get; private set; }

        private ImageSource imageSource = ImageSource.FromUri(new Uri("https://loffers.sklative.com/offers/company-placeholder.png"));
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
            _name = new ValidatableObject<string>();
            _description = new ValidatableObject<string>();
            this.Title = "Publisher";
        }
    }
}
