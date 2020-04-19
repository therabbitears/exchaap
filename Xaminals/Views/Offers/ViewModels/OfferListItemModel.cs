using System;
using System.Collections.Generic;
using Xaminals.Models;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.ViewModels
{
    /// <summary>
    /// OfferListItemModel
    /// </summary>
    public partial class OfferListItemViewModel : BaseViewModel, IOfferViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Terms { get; set; }
        public string Image { get; set; }
        public string OfferToken { get; set; }
        public string LocationToken { get; set; }
        public string PublisherName { get; set; }
        public string PublisherLogo { get; set; }
        public string PublisherToken { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string SubPublisherLogo { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public Coordinates Coordinates { get; set; }
        public CoordinatesDistance Distance { get; set; }
        
        private bool _Starred;
        public bool Starred
        {
            get { return _Starred; }
            set { _Starred = value; OnPropertyChanged("Starred"); }
        }
    }

    public class CoordinatesDistance
    {
        public long Distance { get; set; }
        public string DistanceIn { get; set; }
    }

    public class Coordinates
    {
        public Coordinates()
        {
        }

        public Coordinates(double latVal, double longVal)
        {
            this.Lat = latVal;
            this.Long = longVal;
        }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
