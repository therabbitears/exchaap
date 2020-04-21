namespace Xaminals.Views.Categories.Models.DTO
{
    public class CategoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string Image { get; set; }
        public string ParentId { get; set; }
        public bool IsParent { get { return string.IsNullOrEmpty(ParentId); } }
    }
}
