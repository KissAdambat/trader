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
    /// Interaction logic for Admin.xaml
    /// </summary>
    /// 
    
    public partial class Admin : Page
    {
        private readonly DataBaseStatemenst DataBaseStatemenst = new DataBaseStatemenst();
        private readonly MainWindow mainWindow;
        public Admin()
        {
            InitializeComponent();
            UsersDataGrid.ItemsSource = DataBaseStatemenst.GetAllUsers();
        }
    }
}
