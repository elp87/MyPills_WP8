using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using My_Pills.Classes;
using My_Pills.Common;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace My_Pills
{
    /// <summary>
    ///     Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingPage
    {
        private readonly NavigationHelper _navigationHelper;
        private ObservableCollection<string> _periodCollection;
        private XElement _xml;

        public SettingPage()
        {
            InitializeComponent();

            PeriodsListView.ItemsSource = _periodCollection;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
        }

        #region Регистрация NavigationHelper
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var param = e.NavigationParameter as SettingPageNavigationParameter;
            if (param != null)
            {
                _xml = param.PillsXml;
                _periodCollection = new ObservableCollection<string>(DataHelper.GetPeriodsFromXml(_xml));
                PeriodsListView.ItemsSource = _periodCollection;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AddPeriodContentDialog dialog = new AddPeriodContentDialog();
            var dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Primary)
            {
                string periodName = dialog.PeriodName;
                _periodCollection.Add(periodName);

                XmlFile.AddPeriod(_xml, periodName);
            }
        }

        private void OkAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (_xml != null)
            {
                XmlFile.Save(_xml);
                Frame.GoBack();
            }
        }

        private void ListViewItemGrid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void DeleteFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedPeriod = ((FrameworkElement) e.OriginalSource).DataContext as string;
            if (selectedPeriod != null)
            {
                _periodCollection.Remove(selectedPeriod);
                XmlFile.RemovePeriod(_xml, selectedPeriod);
            }
            
        }
    }
}