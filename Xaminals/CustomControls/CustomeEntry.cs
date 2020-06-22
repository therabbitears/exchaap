using System;
using Xamarin.Forms;

namespace exchaup.CustomControls
{
    public class CustomeEntry : Entry
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
                                propertyName: "Padding",
                                returnType: typeof(Thickness),
                                declaringType: typeof(CustomeEntry),
                                defaultValue: new Thickness(0, 0, 0, 15),
                                defaultBindingMode: BindingMode.TwoWay,
                                propertyChanged: onProperyChanged
        );

        private static void onProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomeEntry targetView;

            targetView = (CustomeEntry)bindable;
            if (targetView != null)
                targetView.Padding = (Thickness)newValue;
        }

        public Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
                OnPropertyChanged("Padding");
            }
        }
    }
}
