using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly DataBaseStatemenst DataBaseStatemenst = new DataBaseStatemenst();
        private readonly MainWindow mainWindow;
        public Login(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void LogButtonClick(object sender, RoutedEventArgs e)
        {
            var user = new
            {
                username = UsernameLoginTextBox.Text,
                password = LoginPasswordBox.Password
            };
            MessageBox.Show(DataBaseStatemenst.LoginUser(user).ToString());
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.StartWindow.Navigate(new RegisterPage());
        }
    }
}
