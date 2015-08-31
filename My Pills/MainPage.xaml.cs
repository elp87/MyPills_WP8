using My_Pills.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace My_Pills
{
    public sealed partial class MainPage : Page
    {
        private Windows.System.Display.DisplayRequest _keepScreenOnRequest = null;
        private XElement _pillsXML = null;        

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
                await XmlFile.Save(XmlFile.Default);
            }
            AppPivot.Items.Clear();

            _pillsXML = await XmlFile.Read();

            IEnumerable<string> periods = _pillsXML.Descendants("time").Select(x => x.Attribute("name").Value);
            foreach (string period in periods)
            {
                PivotItem pivotItem = new PivotItem() { Header = period };
                PillsListView pillListView = new PillsListView(Classes.DataHelper.GetPillsFromXML(_pillsXML, period));
                pivotItem.Content = pillListView;
                AppPivot.Items.Add(pivotItem);
            }
        }

        private void EditPeriodAppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string periodName = ((PivotItem)AppPivot.SelectedItem).Header.ToString();
            List<Classes.Pill> periodPills = Classes.DataHelper.GetPillsFromXML(_pillsXML, periodName);
            this.Frame.Navigate(typeof(EditPeriodPage), 
                                new Classes.EditPeriodPage_NavigationParameter() { periodName = periodName, pills = periodPills }
                                );
        }

        private void PreventLock()
        {
            _keepScreenOnRequest = new Windows.System.Display.DisplayRequest();
            _keepScreenOnRequest.RequestActive();
        }
    }
}
