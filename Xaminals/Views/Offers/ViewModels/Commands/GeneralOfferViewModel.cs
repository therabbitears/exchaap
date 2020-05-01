using System.Collections.Specialized;
using System.Linq;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class GeneralOfferViewModel
    {       
        protected override void AddListeners()
        {
            base.AddListeners();
            this.Categories.CollectionChanged += OnColectionChanged;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            this.Categories.CollectionChanged -= OnColectionChanged;
            RaiseSuccess("Destructor called");
        }

        private void OnColectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Categories.Any())
                this.HasCategoroies = true;
        }
    }
}
