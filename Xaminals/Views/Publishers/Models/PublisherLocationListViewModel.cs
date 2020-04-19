using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Publishers.Models
{
	public partial class PublisherLocationListViewModel : BaseViewModel
    {

		ObservableCollection<PublisherLocationModel> _availableLocations;
		public ObservableCollection<PublisherLocationModel> AvailableLocations
		{
			get { return _availableLocations; }
			set { _availableLocations = value; }
		}

		public PublisherLocationListViewModel()
		{
			Title = "My locations";			
		}

		protected override void IntializeMembers()
		{
			base.IntializeMembers();
			AvailableLocations = new ObservableCollection<PublisherLocationModel>();
			this.Title = "Locations";
		}
	}
}
