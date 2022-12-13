using PL.ProductWindows;
using System.Windows;
using System.Windows.Media;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ErrorMessageWindow.xaml
    /// </summary>
    public partial class ErrorMessageWindow : Window
    {
        /// <summary>
        /// a message window that alerts errors when preforming an action
        /// </summary>
        /// <param name="text">The corresponding message</param>
        public ErrorMessageWindow(string title, string text)
        {
            InitializeComponent();
            Title = title;
            ErrorMessageTextBlock.Text = text;
        }

        /// <summary>
        /// closing window
        /// </summary>
        /// <param name="sender">the ok button</param>
        /// <param name="e">mouse click</param>
        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}