using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace My_Pills
{
    /// <summary>
    ///     Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PeriodOptionsPage : Page
    {
        public PeriodOptionsPage()
        {
            InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        /// <summary>
        ///     Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">
        ///     Данные события, описывающие, каким образом была достигнута эта страница.
        ///     Этот параметр обычно используется для настройки страницы.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}