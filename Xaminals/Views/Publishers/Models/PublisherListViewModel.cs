using System.Collections.ObjectModel;
using Xaminals.ViewModels;

namespace Xaminals.Views.Publishers.Models
{
    /// <summary>
    /// PublisherListViewModel
    /// </summary>
    public partial class PublisherListViewModel : ListBaseViewModel
    {
        ObservableCollection<PublisherListItemViewModel> _publishers;
        public ObservableCollection<PublisherListItemViewModel> Publishers
        {
            get
            {
                return _publishers;
            }
            set
            {
                _publishers = value;
            }
        }

        private PublisherListItemViewModel _selectedItem;

        public PublisherListItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                EditPublisherCommand.Execute(null);
            }
        }


        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _publishers = new ObservableCollection<PublisherListItemViewModel>();
            this.Title = "Publishers";
        }
    }
}
