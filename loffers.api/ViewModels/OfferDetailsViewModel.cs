using Loffers.Server.Data;
using Loffers.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loffers.api.ViewModels
{
    public class OfferDetailsViewModel
    {
        public string Name { get;  set; }
        public string Id { get;  set; }
        public string Detail { get;  set; }
        public string Terms { get;  set; }
        public string Image { get;  set; }
        public string OriginalImage { get;  set; }
        public string OfferToken { get;  set; }
        public DateTime? ValidFrom { get;  set; }
        public CategoryModel Category { get; set; }
        public List<CategoryModel> Categories { get;  set; }
        public CoordinatesDistance Distance { get;  set; }
        public CoordinatesShort Coordinates { get;  set; }
        public string Url { get;set; }
    }
}