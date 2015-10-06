using My_Pills.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace My_Pills
{
    public sealed partial class MainPage
    {
        private Windows.System.Display.DisplayRequest _keepScreenOnRequest;
        private XElement _pillsXml;
        private string _activePivotHeader = "";

        public MainPage()
        {
            InitializeComponent();            

            NavigationCacheMode = NavigationCacheMode.Required;
            PreventLock();
        }        

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            bool fileExists = await XmlFile.IsExists();
            if (!fileExists)
            {
                await XmlFile.SaveAsync(XmlFile.Default);
            }
            ClearAppPivot();

            _pillsXml = await XmlFile.Read();

            List<string> periods = _pillsXml.Descendants("time").Select(x => x.Attribute("name").Value).ToList();
            foreach (string period in periods)
            {
                PivotItem pivotItem = new PivotItem() { Header = period };
                PillsListView pillListView = new PillsListView(DataHelper.GetPillsFromXML(_pillsXml, period));
                pivotItem.Content = pillListView;
                if (AppPivot.Items != null) AppPivot.Items.Add(pivotItem);
            }

            // Запускаем приложение с первого периода
            if (e.NavigationMode == NavigationMode.New)
            {
                _activePivotHeader = periods.First();
            }

            if (_activePivotHeader != "")
            {
                AppPivot.SelectedItem = AppPivot.Items.FirstOrDefault(item =>
                {
                    PivotItem pivotItem = item as PivotItem;
                    return (pivotItem != null && pivotItem.Header.ToString() == _activePivotHeader); // Выбираем PivotItem, у которого Header = _activePivotHeader
                });
            }
        }

        private void ClearAppPivot()
        {
            if (AppPivot.Items == null) return;
            AppPivot.Items.Clear();

            PivotItem basePivotItem = new PivotItem() {Header = "Основное"};
            Grid bpiGrid = new Grid();
            Button optionsButton = new Button()
            {
                Margin = new Thickness(10, 10,10,0), 
                Content = "Настройки", 
                VerticalAlignment = VerticalAlignment.Top, 
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            optionsButton.Click += optionsButton_Click;

            bpiGrid.Children.Add(optionsButton);
            basePivotItem.Content = bpiGrid;
            AppPivot.Items.Add(basePivotItem);

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            PivotItem selectedItem = AppPivot.SelectedItem as PivotItem;
            if (selectedItem != null)
            {
                _activePivotHeader = selectedItem.Header.ToString();
            }
        }

        private void EditPeriodAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            PivotItem selectedItem = (PivotItem) AppPivot.SelectedItem;
            if (AppPivot.Items != null)
            {
                PivotItem baseItem = (PivotItem) AppPivot.Items[0];
                if (selectedItem == baseItem) return;
            }
            string periodName = selectedItem.Header.ToString();
            List<Pill> periodPills = DataHelper.GetPillsFromXML(_pillsXml, periodName);
            Frame.Navigate(typeof (EditPeriodPage),
                           new EditPeriodPage_NavigationParameter() {periodName = periodName, pills = periodPills}
                           );
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingPage),
                           new SettingPageNavigationParameter() { PillsXml = _pillsXml }
                           );
        }

        private void PreventLock()
        {
            _keepScreenOnRequest = new Windows.System.Display.DisplayRequest();
            _keepScreenOnRequest.RequestActive();
        }

        private void AppPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BottomAppBar == null || AppPivot.Items == null) return;
            if (AppPivot.SelectedItem != AppPivot.Items[0]) BottomAppBar.Visibility = Visibility.Visible;
            else BottomAppBar.Visibility = Visibility.Collapsed;
        }
    }
}
