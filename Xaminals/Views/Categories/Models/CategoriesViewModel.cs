using System.Collections.Generic;
using Xaminals.Services.HttpServices;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Categories.Models
{
    public class CategoriesViewModel : BaseViewModel
    {
        #region Lists

        List<CategoryModel> _categories;
        public List<CategoryModel> Categories
        {
            get
            {
                // _categories = new RestService().OfferCategories().Result;
                return _categories;
            }
        }


        #endregion
    }
}
