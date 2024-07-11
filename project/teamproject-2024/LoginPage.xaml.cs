using System.Windows;
using MahApps.Metro.Controls;

namespace teamproject_2024
{
    public partial class LoginPage : MetroWindow
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string id = txtID.Text;
            string password = txtPassword.Text;

            if (IsValidUser(id, password))
            {
                MessageBox.Show("로그인 성공!");
            }
            else
            {
                MessageBox.Show("로그인 실패. 다시 시도하세요.");
            }
        }

        private bool IsValidUser(string id, string password)
        {
            return id == "asdf" && password == "1234";
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Text = "Password";
                txtPassword.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 윈도우 로드 시 처리할 로직
        }
    }
}
