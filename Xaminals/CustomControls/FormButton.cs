using Xamarin.Forms;

namespace Loffers.CustomControls
{
    public class FormButton : Button
    {
        public FormButton()
        {
            this.Clicked += OnClicked;
        }


        private void OnClicked(object sender, System.EventArgs e)
        {
            if (this.FocusedItem != null)
                this.FocusedItem.Focus();
        }

        public static readonly BindableProperty HasItems = BindableProperty.Create(
                              propertyName: "FocusedItem",
                              returnType: typeof(Entry),
                              declaringType: typeof(FormButton),
                              defaultValue: null,
                              defaultBindingMode: BindingMode.TwoWay,
                              propertyChanged: onProperyChanged
      );

        private static void onProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // ----- Someone changed the full control's Value property. Store
            //       that new value in the internal Switch's IsToggled property.
            FormButton targetView;

            targetView = (FormButton)bindable;
            if (targetView != null)
                targetView.FocusedItem = (Entry)newValue;
        }

        public Entry FocusedItem
        {
            get
            {
                return (Entry)GetValue(HasItems);
            }
            set
            {
                SetValue(HasItems, value);
                OnPropertyChanged("FocusedItem");
            }
        }
    }
}
