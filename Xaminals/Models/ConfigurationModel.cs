using System.Collections.Generic;
using Xaminals.Views.Categories.Models.DTO;

namespace Loffers.Models
{
    public class ConfugurationModel
    {
        public bool IsPublisher { get; set; }
        public int UnitOfMeasurement { get; set; }
        public int MaxRange { get; set; }
    }

    public class SettingsModel 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ConfugurationModel Configuration { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}