using My_Pills.Classes;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace My_Pills
{
    public sealed partial class PillsListView : UserControl
    {
        public PillsListView()
        {
            this.InitializeComponent();
        }

        public PillsListView(List<Pill> pills) : this()
        {
            this.PillList.ItemsSource = pills;
        }
    }
}
