using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace exchaup.CustomControls
{
    public class ExtenderListView : ListView
    {
        #region Properties

        public static readonly BindableProperty AnyItemProperty = BindableProperty.Create(
                                 propertyName: "AnyItem",
                                 returnType: typeof(bool),
                                 declaringType: typeof(ExtenderListView),
                                 defaultValue: false,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: onProperyChanged
         );

        public static readonly BindableProperty OnTappedCommandProperty = BindableProperty.Create(
                                 propertyName: "OnTappedCommand",
                                 returnType: typeof(ICommand),
                                 declaringType: typeof(ExtenderListView),
                                 defaultValue: null,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: onTappedProperyChanged
         );

        public static readonly BindableProperty OnDataRequiredCommandProperty = BindableProperty.Create(
                                 propertyName: "OnDataRequiredCommand",
                                 returnType: typeof(ICommand),
                                 declaringType: typeof(ExtenderListView),
                                 defaultValue: null,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: onOnDataRequiredCommandProperyChanged
         );

        public static readonly BindableProperty CurrentPageNumberProperty = BindableProperty.Create(
                                 propertyName: "CurrentPageNumber",
                                 returnType: typeof(int),
                                 declaringType: typeof(ExtenderListView),
                                 defaultValue: 0,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: onPageNumberProperyChanged
         );

        private static void onTappedProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // ----- Someone changed the full control's Value property. Store
            //       that new value in the internal Switch's IsToggled property.
            ExtenderListView targetView;

            targetView = (ExtenderListView)bindable;
            if (targetView != null)
                targetView.OnTappedCommand = (ICommand)newValue;
        }

        private static void onOnDataRequiredCommandProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // ----- Someone changed the full control's Value property. Store
            //       that new value in the internal Switch's IsToggled property.
            ExtenderListView targetView;

            targetView = (ExtenderListView)bindable;
            if (targetView != null)
                targetView.OnDataRequiredCommand = (ICommand)newValue;
        }

        private static void onProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // ----- Someone changed the full control's Value property. Store
            //       that new value in the internal Switch's IsToggled property.
            ExtenderListView targetView;

            targetView = (ExtenderListView)bindable;
            if (targetView != null)
                targetView.AnyItem = (bool)newValue;
        }

        private static void onPageNumberProperyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtenderListView targetView;

            targetView = (ExtenderListView)bindable;
            if (targetView != null)
                targetView.CurrentPageNumber = (int)newValue;
        }

        public bool AnyItem
        {
            get
            {
                return (bool)GetValue(AnyItemProperty);
            }
            set
            {
                SetValue(AnyItemProperty, value);
                OnPropertyChanged("AnyItem");
            }
        }

        public int CurrentPageNumber
        {
            get
            {
                return (int)GetValue(CurrentPageNumberProperty);
            }
            set
            {
                SetValue(CurrentPageNumberProperty, value);
                OnPropertyChanged("CurrentPageNumber");
            }
        }

        public ICommand OnTappedCommand
        {
            get
            {
                return (ICommand)GetValue(OnTappedCommandProperty);
            }
            set
            {
                SetValue(OnTappedCommandProperty, value);
                OnPropertyChanged("OnTappedCommand");
            }
        }

        public ICommand OnDataRequiredCommand
        {
            get
            {
                return (ICommand)GetValue(OnDataRequiredCommandProperty);
            }
            set
            {
                SetValue(OnDataRequiredCommandProperty, value);
                OnPropertyChanged("OnDataRequiredCommand");
            }
        }

        private object _lastVisibleItem;
        public object LastVisibleItem
        {
            get { return _lastVisibleItem; }
            set { _lastVisibleItem = value; }
        }

        #endregion

        #region Mutators

        #endregion

        public ExtenderListView()
        {
            this.ItemTapped += OnItemTapped;
            this.ItemAppearing += OnScrolled;
        }

        private void OnScrolled(object sender, ItemVisibilityEventArgs e)
        {
            var source = (this.ItemsSource as IList);
            if (e.Item == source[source.Count - 1])
            {
                if (LastVisibleItem != e.Item)
                {
                    CurrentPageNumber++;
                    LastVisibleItem = e.Item;
                    OnDataRequiredCommand?.Execute(this);
                }
            }
        }

        public ExtenderListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            this.ItemTapped += OnItemTapped;
            this.ItemAppearing += OnScrolled;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (this.OnTappedCommand != null)
                this.OnTappedCommand.Execute(e.Item);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (ItemsSource != null && ItemsSource.GetEnumerator().Current != null)
                AnyItem = true;
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
            if (ItemsSource == null || ItemsSource.GetEnumerator().Current == null)
                AnyItem = false;
        }
    }
}
