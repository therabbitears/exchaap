namespace Loffers.Server.ViewModels
{
    /// <summary>
    /// PublisherViewModel
    /// </summary>
    public class PublisherViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class PublisherLocationViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string DisplayAddress { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Id { get; set; }
    }
}
