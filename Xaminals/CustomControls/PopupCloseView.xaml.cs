using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loffers.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupCloseView : ContentView
    {
        public PopupCloseView()
        {
            InitializeComponent();
        }
    }
}