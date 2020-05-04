using System.Collections.Specialized;
using Xamarin.Forms;

namespace exchaup.CustomControls
{
    public class BottomScrolledListView : ListView
    {
        public BottomScrolledListView()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (this.ItemsSource is INotifyCollectionChanged list)
                {
                    list.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ScrollTo(e.NewItems[0], ScrollToPosition.End, true);
        }
    }
}
