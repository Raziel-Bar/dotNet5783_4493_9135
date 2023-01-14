using PL.ProductWindows;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for AdminVerificationWindow.xaml
/// </summary>
public partial class AdminVerificationWindow : Window
{
    /// <summary>
    /// The correct password for the administrator.
    /// </summary>
    private string password = "1234";
    public bool isVerified = false;

    public AdminVerificationWindow()
    {
        InitializeComponent();
    }


    /// <summary>
    /// Verifies the password entered by the user.
    /// </summary>
    /// <param name="sender">The password input box.</param>
    /// <param name="e">The click event arguments.</param>
    private void VerifyPassword_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        PasswordBox passwordBox = (PasswordBox)button.Tag;
        string pwInput = passwordBox.Password;
        if (pwInput != password)
        {
            SystemSounds.Beep.Play();
        }
        else
        {
            isVerified = true;
            new AdminWindow().Show();
            this.Close();
        }
    }

}
