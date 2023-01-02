using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    public class ErrorMessageWindowData
    {
        public string? TextMessage { get; set; }
        public string? Title { get; set; }
    }

    /// <summary>
    /// Interaction logic for ErrorMessageWindow.xaml
    /// </summary>
    public partial class ErrorMessageWindow : Window
    {
        public ErrorMessageWindowData Data { get; set; }
        /// <summary>
        /// a message window that alerts errors when preforming an action
        /// </summary>
        /// <param name="text">The corresponding message</param>
        public ErrorMessageWindow(string title, string text)
        {
            SystemSounds.Hand.Play();

            Data = new()
            {
                TextMessage = text,
                Title = title
            };
            InitializeComponent();
        }

        /// <summary>
        /// closing window
        /// </summary>
        /// <param name="sender">the ok button</param>
        /// <param name="e">mouse click</param>
        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}
