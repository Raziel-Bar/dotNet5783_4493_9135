using System.Windows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for SuccessWindow.xaml
    /// </summary>
    public partial class SuccessWindow : Window
    {
        public SuccessWindow(string text)
        {
            InitializeComponent();
            SuccessTextBlock.Text = text;
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
