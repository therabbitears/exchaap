using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exchaup.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndicatingSearch : ContentView
    {
        public IndicatingSearch()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                propertyName: "Title",
                                returnType: typeof(string),
                                declaringType: typeof(IndicatingSearch),
                                defaultValue: string.Empty,
                                defaultBindingMode: BindingMode.TwoWay,
                                propertyChanged: onProperyChanged
        );

        private static void onProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            IndicatingSearch targetView;

            targetView = (IndicatingSearch)bindable;
            if (targetView != null)
                targetView.Title = (string)newValue;
        }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
                this.txtQuery.Placeholder = value;
                OnPropertyChanged("Title");
            }
        }
    }
}