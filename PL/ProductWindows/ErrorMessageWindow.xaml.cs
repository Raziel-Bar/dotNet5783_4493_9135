using System.Windows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ErrorMessageWindow.xaml
    /// </summary>
    public partial class ErrorMessageWindow : Window
    {
        public ErrorMessageWindow(string text)
        {
            InitializeComponent();
            ErrorMessageTextBlock.Text = text;
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}
