﻿namespace exchaup.Views.Offer_Public.Models
{
    public class SearchLocationItemViewModel
    {
        public string Name { get; set; }
        public string Landmark { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public bool IsCurrent { get; internal set; }
    }
}
