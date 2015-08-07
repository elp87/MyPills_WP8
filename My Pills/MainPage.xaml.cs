﻿using System;
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
        private Windows.System.Display.DisplayRequest _keepScreenOnRequest = null;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            PreventLock();
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
            IEnumerable<string> periods = xe.Descendants("time").Select(x => x.Attribute("name").Value);
            foreach (string period in periods)
            {
                PivotItem pivotItem = new PivotItem() { Header = period };
                PillsListView pillListView = new PillsListView(
                    xe.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == period)
                                    .Descendants("item")
                                    .Select(s => new Pill(
                                        name: s.Element("name").Value,
                                        info: s.Element("info") != null ? s.Element("info").Value : "")
                                        )
                                    .ToList()
                    );
                pivotItem.Content = pillListView;
                AppPivot.Items.Add(pivotItem);
            }
        }

        private void PreventLock()
        {
            _keepScreenOnRequest = new Windows.System.Display.DisplayRequest();
            _keepScreenOnRequest.RequestActive();
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string periodName = ((PivotItem)AppPivot.SelectedItem).Header.ToString();             
            
        }
    }
}
