using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace trader
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private readonly DataBaseStatemenst db = new DataBaseStatemenst();
        private readonly MainWindow mainWindow;
        Connect conn = new Connect();
        public RegisterPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == PasswordBoxAgain.Password)
            {
                var result = db.AddNewUser(new
                {
                    username = UsernameTextBox.Text,
                    fullname = FullName.Text,
                    password = PasswordBox.Password,
                    salt = "",
                    email = Email.Text
                });
                MessageBox.Show(result.ToString());
                mainWindow.StartWindow.Navigate(new Login(mainWindow));
            }
            else
            {
                MessageBox.Show("A jelszavak nem egyeznek.");
            }
        }
    }
}
