namespace Xaminals.ViewModels
{
    public class ListBaseViewModel : BaseViewModel
    {
        private bool _hasItems = true;
        public bool HasItems
        {
            get { return _hasItems; }
            set
            {
                if (_hasItems != value)
                {
                    _hasItems = value; OnPropertyChanged("HasItems");
                }
            }
        }

        private int _currentPageNumber;
        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set { SetProperty(ref _currentPageNumber, value); }
        }
    }
}
