using System;
using Xamarin.Forms;

namespace exchaup.CustomControls
{
    public class FormButton : Button
    {
        public FormButton()
        {
            this.Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            if (this.FocusedItem != null)
                this.FocusedItem.Focus();

            this.Command?.Execute(this.CommandParameter);
        }

        public static readonly BindableProperty HasItems = BindableProperty.Create(
                              propertyName: "FocusedItem",
                              returnType: typeof(Editor),
                              declaringType: typeof(FormButton),
                              defaultValue: null,
                              defaultBindingMode: BindingMode.TwoWay,
                              propertyChanged: onProperyChanged
                    );

        private static void onProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FormButton targetView;
            targetView = (FormButton)bindable;
            if (targetView != null)
                targetView.FocusedItem = (Editor)newValue;
        }

        public Editor FocusedItem
        {
            get
            {
                return (Editor)GetValue(HasItems);
            }
            set
            {
                SetValue(HasItems, value);
                OnPropertyChanged("FocusedItem");
            }
        }
    }
}
