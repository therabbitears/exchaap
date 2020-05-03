using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Data
{
    public class DataStore
    {
        public static List<CategoryModel> Categories()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<CategoryModel>>(File.ReadAllText("categories.txt"));
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new List<CategoryModel>();
        }
    }
}