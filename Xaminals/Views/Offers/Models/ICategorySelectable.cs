using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.Models
{
    public interface ISelectable
    {

    }

    public interface ICategoriesSelectable : ISelectable
    {
        ObservableCollection<CategoryModel> Categories { get; set; }
        int MaxAllowed { get; }
    }

    public interface ICategorySelectable : ISelectable
    {
        CategoryModel Category { get; set; }
    }
}