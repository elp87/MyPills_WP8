using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace My_Pills
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
                
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            Uri dataUri = new Uri("ms-appx:///Data/pills.xml");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string text = await FileIO.ReadTextAsync(file);

            XElement xe = XElement.Parse(text, LoadOptions.None);
            /*List<string> morning = xe.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == "morning")
                                    .Descendants("item")
                                    .Select(s => s.Value)
                                    .ToList();
            this.morningListView.ItemsSource = morning;*/
            List<Pill> morning = xe.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == "morning")
                                    .Descendants("item")
                                    .Select(s => new Pill(
                                        name: s.Element("name").Value,
                                        info: s.Element("info") != null ? s.Element("info").Value : "")
                                        )
                                    .ToList();
            this.morningListView.ItemsSource = morning;

            List<string> afternoon = xe.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == "afternoon")
                                    .Descendants("item")
                                    .Select(s => s.Value)
                                    .ToList();
            this.afternoonListView.ItemsSource = afternoon;

            List<string> evening = xe.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == "evening")
                                    .Descendants("item")
                                    .Select(s => s.Value)
                                    .ToList();
            this.eveningListView.ItemsSource = evening;
        }
    }
}
