using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Infra.Mappers
{
    public class Mapper
    {
        public static void Map(OfferListItemViewModel source, GeneralOfferViewModel destination)
        {
            if (source != null)
            {
                destination.Detail = source.Detail;
                destination.Terms = source.Terms;
                destination.Name = source.Name;
                destination.Distance = source.Distance.Distance;
                destination.Id = source.Id;
                destination.Image = source.Image;
                destination.LocationAddress = source.LocationAddress;
                destination.LocationName = source.LocationName;
                destination.PublisherLogo = source.PublisherLogo;
                destination.SubPublisherLogo = source.SubPublisherLogo;
                destination.OfferToken = source.OfferToken;
                destination.PublisherName = source.PublisherName;
                destination.PublisherToken = source.PublisherToken;
                destination.LocationId = source.LocationToken;
                destination.ValidFrom = source.ValidFrom;
                destination.ValidTill = source.ValidTill;
                destination.Coordinates = destination.Coordinates ?? new Coordinates();

                destination.Coordinates.Lat = source.Coordinates.Lat;
                destination.Coordinates.Long = source.Coordinates.Long;
                destination.Starred = source.Starred;
                destination.Category = destination.Category ?? new CategoryModel();
                destination.Category.Id = source.Category.Id;
                destination.Category.Name = source.Category.Name;
                destination.Category.Image = source.Category.Image;
                destination.Category.Selected = true;

                destination.Categories = destination.Categories ?? new ObservableCollection<CategoryModel>();
                destination.Categories.Clear();
                foreach (var item in source.Categories)
                {
                    destination.Categories.Add(item);
                }
            }
        }
    }
}
