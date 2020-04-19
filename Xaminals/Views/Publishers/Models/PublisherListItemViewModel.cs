using System.Collections.Generic;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Publishers.Models
{
    public class PublisherListItemViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Id { get; set; }
        public List<PublisherLocationModel> Locations { get; set; }
    }
}
