using System;
using System.Collections.Generic;

namespace Loffers.Server.Data
{
    public class OfferModel
    {
        public string Heading { get; set; }
        public string Detail { get; set; }
        public string Terms { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Image { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<OfferLocationModel> OfferLocations { get; set; }
        public string Id { get; set; }
    }
}
