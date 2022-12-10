using System.Windows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for SuccessWindow.xaml
    /// </summary>
    public partial class SuccessWindow : Window
    {
        /// <summary>
        /// a message window that confirms the success of an action
        /// </summary>
        /// <param name="text">The corresponding message</param>
        public SuccessWindow(string text)
        {
            InitializeComponent();
            SuccessTextBlock.Text = text;
        }

        /// <summary>
        /// closing window
        /// </summary>
        /// <param name="sender">the ok button</param>
        /// <param name="e">mouse click</param>
        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
        
    }
}
