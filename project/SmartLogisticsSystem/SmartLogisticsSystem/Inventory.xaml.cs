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
using System.Windows.Shapes;

namespace SmartLogisticsSystem
{
    /// <summary>
    /// inventory.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Inventory : Window
    {
        public Inventory()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void GrdResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // 실시간조회 버튼 클릭
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
