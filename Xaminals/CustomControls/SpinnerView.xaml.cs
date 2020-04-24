using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exchaup.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpinnerView : ContentView
    {
        public SpinnerView()
        {
            InitializeComponent();
        }
    }
}