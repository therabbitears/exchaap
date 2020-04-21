using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.Models
{
    public interface ICategorySelectable
    {
        ObservableCollection<CategoryModel> Categories { get; set; }
        CategoryModel Category { get; set; }
    }
}