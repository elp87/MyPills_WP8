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
    public sealed partial class MainPage : Page
    {
        private Windows.System.Display.DisplayRequest _keepScreenOnRequest = null;
        private XElement _pillsXML = null;
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

            _pillsXML = await XmlFile.Read();

            IEnumerable<string> periods = _pillsXML.Descendants("time").Select(x => x.Attribute("name").Value);
            foreach (string period in periods)
            {
                PivotItem pivotItem = new PivotItem() { Header = period };
                PillsListView pillListView = new PillsListView(Classes.DataHelper.GetPillsFromXML(_pillsXML, period));
                pivotItem.Content = pillListView;
                AppPivot.Items.Add(pivotItem);
            }

            if (_activePivotHeader != "")
            {
                AppPivot.SelectedItem = AppPivot.Items.FirstOrDefault(item =>
                {
                    PivotItem pivotItem = item as PivotItem;
                    if (pivotItem.Header.ToString() == _activePivotHeader) return true;
                    else return false;
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
            PivotItem selectedItem = this.AppPivot.SelectedItem as PivotItem;
            if (selectedItem != null){
                _activePivotHeader = selectedItem.Header.ToString();
            }
        }

        private void EditPeriodAppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PivotItem selectedItem = (PivotItem) AppPivot.SelectedItem;
            if (AppPivot.Items != null)
            {
                PivotItem baseItem = (PivotItem) AppPivot.Items[0];
                if (selectedItem == baseItem) return;
            }
            string periodName = selectedItem.Header.ToString();
            List<Classes.Pill> periodPills = Classes.DataHelper.GetPillsFromXML(_pillsXML, periodName);
            this.Frame.Navigate(typeof (EditPeriodPage),
                new Classes.EditPeriodPage_NavigationParameter() {periodName = periodName, pills = periodPills}
                );
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage),
                                new SettingPageNavigationParameter() { PillsXml = _pillsXML }
                                );
        }

        private void PreventLock()
        {
            _keepScreenOnRequest = new Windows.System.Display.DisplayRequest();
            _keepScreenOnRequest.RequestActive();
        }
    }
}
