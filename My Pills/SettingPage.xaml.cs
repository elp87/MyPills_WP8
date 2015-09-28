using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using My_Pills.Classes;
using My_Pills.Common;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace My_Pills
{
    /// <summary>
    ///     Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        private readonly NavigationHelper _navigationHelper;
        private ObservableCollection<string> _periodCollection;

        public SettingPage()
        {
            InitializeComponent();

            PeriodsListView.ItemsSource = _periodCollection;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var param = e.NavigationParameter as SettingPageNavigationParameter;
            if (param != null)
            {
                var xml = param.PillsXml;
                _periodCollection = new ObservableCollection<string>(DataHelper.GetPeriodsFromXml(xml));
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
    }
}