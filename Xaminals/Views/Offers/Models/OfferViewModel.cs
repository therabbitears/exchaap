﻿using Plugin.Media.Abstractions;
using Plugin.ValidationRules;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.Models
{
    [QueryProperty("Id", "offerid")]
    public partial class OfferViewModel : BaseViewModel
    {
        private ValidatableObject<string> _heading;
        public ValidatableObject<string> Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }


        private ValidatableObject<string> _detail;
        public ValidatableObject<string> Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }


        private ValidatableObject<string> _terms;

        public ValidatableObject<string> Terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        private ValidatableObject<DateTime?> _validfrom;
        public ValidatableObject<DateTime?> Validfrom
        {
            get { return _validfrom; }
            set { _validfrom = value; }
        }

        private bool _isGiveUp;
        public bool IsGiveUp
        {
            get { return _isGiveUp; }
            set { SetProperty(ref _isGiveUp, value); }
        }

        private Stream _imageStream;
        public Stream ImageStream
        {
            get { return _imageStream; }
            set
            {
                _imageStream = value;
                OnPropertyChanged("ImageStream");
            }
        }

        private ImageSource imageSource = ImageSource.FromUri(new Uri("https://loffers.sklative.com/offers/offer-placeholder.png"));
        public ImageSource SourceImage
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged("SourceImage"); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _id)
                {
                    _id = newValue;
                    OnPropertyChanged("Id");
                }
            }
        }

        private MediaFile _mediaFile;

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _heading = new ValidatableObject<string>();
            _detail = new ValidatableObject<string>();
            _terms = new ValidatableObject<string>();
            _validfrom = new ValidatableObject<DateTime?>();
            _validfrom.Value = DateTime.Now;
            _categories = new ObservableCollection<CategoryModel>();
            _locations = new ObservableCollection<OfferPublisherLocationModel>();
            this.Title = "Offer";
        }

        #region Lists

        ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }

        ObservableCollection<OfferPublisherLocationModel> _locations;
        public ObservableCollection<OfferPublisherLocationModel> Locations
        {
            get
            {
                return _locations;
            }
            set
            {
                _locations = value;
            }
        }

        #endregion
    }
}
