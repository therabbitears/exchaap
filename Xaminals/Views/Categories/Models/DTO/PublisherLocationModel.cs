namespace Xaminals.Views.Categories.Models.DTO
{
    public class PublisherLocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayAddress { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }

    public class OfferPublisherLocationModel : PublisherLocationModel
    {
        public bool Selected { get; set; }
    }
}
