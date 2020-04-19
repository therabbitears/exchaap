using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class OfferComplaints
    {
        public long ReportId { get; set; }
        public long OfferId { get; set; }
        public string ReportContent { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual Offers Offer { get; set; }
    }
}
