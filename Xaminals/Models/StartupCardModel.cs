using Xaminals.Models;
namespace exchaup.Models
{
    public class StartupCardModel : NotificableObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Image"); }
        }

        public bool ShowGoButton { get; set; }
        public string ShortTitle { get; set; }

        private bool _IsFirstScreen;
        public bool IsFirstScreen
        {
            get { return _IsFirstScreen; }
            set { _IsFirstScreen = value; OnPropertyChanged("IsFirstScreen"); }
        }

        private bool _IsSecondScreen;
        public bool IsSecondScreen
        {
            get { return _IsSecondScreen; }
            set { _IsSecondScreen = value; OnPropertyChanged("IsSecondScreen"); }
        }
    }

    public class CategoryStartupCardModel : StartupCardModel 
    {
    }
}