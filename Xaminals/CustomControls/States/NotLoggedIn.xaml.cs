using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loffers.CustomControls.States
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotLoggedIn : ContentView
    {
        public NotLoggedIn()
        {
            InitializeComponent();
        }
    }
}