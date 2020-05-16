namespace Xaminals.Views.Categories.Models.DTO
{
    public interface ILocation
    {
        string Name { get; set; }
        string DisplayAddress { get; set; }
        double Lat { get; set; }
        double Long { get; set; }
        bool IsCurrent { get; set; }
    }
}