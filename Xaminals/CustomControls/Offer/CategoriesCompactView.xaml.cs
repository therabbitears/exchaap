using System.Collections;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exchaup.CustomControls.Offer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesCompactView : ContentView
    {
        public CategoriesCompactView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            if (!container.Children.Any())
            {
                var data = this.BindingContext as IList;
                if (data != null && data.Count>0)
                {
                    var category = new SelectedCategory() { BindingContext = data[0] };
                    this.container.Children.Add(category);

                    if (data.Count > 1)
                    {
                        var label = new Label() { Text = string.Format("+ {0} more", (data.Count - 1).ToString()), VerticalOptions = LayoutOptions.CenterAndExpand };
                        label.StyleClass = new string[1];
                        label.StyleClass[0] = "DimText";
                        this.container.Children.Add(label);
                    }
                }
            }
        }
    }
}