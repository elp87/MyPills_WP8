using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace My_Pills
{
    public sealed partial class AddPeriodContentDialog : ContentDialog
    {
        public AddPeriodContentDialog()
        {
            InitializeComponent();
        }

        public string PeriodName { get; private set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            PeriodName = PeriodNameTextBox.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}