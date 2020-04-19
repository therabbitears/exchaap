using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using Xamarin.Forms.Maps;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class OfferMapListViewModel : OffersListViewModel
    {
        //[Obsolete("We need to remove this as this is just for current case.")]
        //private ObservableCollection<Pin> _Pins;
        //public ObservableCollection<Pin> Pins
        //{
        //    get { return _Pins; }
        //    set { _Pins = value; }
        //}

        private Coordinates _myLocation;
        public Coordinates MyLocation
        {
            get { return _myLocation; }
            set { _myLocation = value; OnPropertyChanged("MyLocation"); }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            //Pins = new ObservableCollection<Pin>();
        }

        protected override void PopulateViewContext(List<OfferListItemViewModel> result, bool isReferesh)
        {
            //_Pins.Clear();
            //foreach (var item in result)
            //{
            //    if (item.Coordinates != null)
            //    {
            //        _Pins.Add(new Pin() { Position = new Position(item.Coordinates.Lat, item.Coordinates.Long), Label = item.LocationAddress });
            //    }
            //}
        }
    }
}
