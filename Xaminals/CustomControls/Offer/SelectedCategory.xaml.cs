using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exchaup.CustomControls.Offer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedCategory : ContentView
    {
        public SelectedCategory()
        {
            InitializeComponent();
        }

        #region Properties

        private bool _showColor = true;
        public bool ShowColor
        {
            get { return _showColor = true; }
            set { SetShowColor(value); }
        }

        private Thickness _innerPadding;
        public Thickness InnerPadding
        {
            get { return _innerPadding; }
            set { SetInnerPadding(value); }
        }

        private Thickness _innerMargin;
        public Thickness InnerMargin
        {
            get { return _innerMargin; }
            set { SetInnerMargin(value); }
        } 
        
        #endregion

        #region Mutators

        private void SetShowColor(bool value)
        {
            if (_showColor != value)
            {
                _showColor = value;
                if (!_showColor)
                    this.parent.BackgroundColor = Color.Transparent;
            }
        }

        private void SetInnerPadding(Thickness value)
        {
            if (_innerPadding != value)
            {
                _innerPadding = value;
                this.parent.Padding = value;
            }
        }

        private void SetInnerMargin(Thickness value)
        {
            if (_innerMargin != value)
            {
                _innerMargin = value;
                this.parent.Margin = value;
            }
        } 

        #endregion
    }
}