using MahApps.Metro.Controls;
using SmartLogisticsSystem;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace teamproject_2024
{
    public partial class MainWindow : MetroWindow
    {
        private static int iNum = 0;
        
        public MainWindow()
        {
            InitializeComponent();

            //MainFrame.LayoutUpdated += MainFrame_LayoutUpdated;
            MainFrame.Navigated += MainFrame_Navigated;
        }


        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        // 네비게이션서비스로 화면 바뀔때마다 타이틀 변경
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            iNum++;
            Debug.WriteLine($"Page {iNum}");
            var contentName = MainFrame.Content.ToString();
            var pageNames = contentName.Split('.');

            switch (pageNames[1].ToUpper())
            {
                case "DBADDPAGE":
                    TxtTitle.Text = "상품 등록";
                    break;
                case "MAINPAGE":
                    TxtTitle.Text = "스마트 물류 서비스";
                    break;
                case "INVENTORYPAGE":
                    TxtTitle.Text = "상품 조회";
                    break;
                case "GRAPHPAGE":
                    TxtTitle.Text = "재고 현황";
                    break;
                default:
                    TxtTitle.Text = "스마트 물류 서비스";
                    break;
            }
        }
    }
}