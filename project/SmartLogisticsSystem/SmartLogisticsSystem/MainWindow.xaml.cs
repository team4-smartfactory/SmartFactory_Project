using MahApps.Metro.Controls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartLogisticsSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            DbAddWindow dbAddWindow = new DbAddWindow();
            dbAddWindow.Show();
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
    }
}