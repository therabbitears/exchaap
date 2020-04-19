using System.Collections.Generic;

namespace Loffers.Server.Data
{
    /// <summary>
    /// OfferReportModel
    /// </summary>
    public class OfferReportModel
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public List<ReportOptionModel> Options { get; set; }
    }

    /// <summary>
    /// ReportOptionModel
    /// </summary>
    public class ReportOptionModel { public string Text { get; set; } }
}