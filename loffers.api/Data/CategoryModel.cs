namespace Loffers.Server.Data
{
    public class CategoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class OfferLocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string DisplayAddress { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
    }

    public class OfferCategoryModel : CategoryModel 
    {
        public bool Selected { get; set; }
    }

    public class UserCategoryModel : CategoryModel
    {
        public bool Selected { get; set; }
    }
}