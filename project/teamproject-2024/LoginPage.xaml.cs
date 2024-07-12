using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace teamproject_2024
{
    public partial class LoginPage : MetroWindow
    {
        public LoginPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen; // 화면 중앙에 표시되도록 설정
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async void Login()
        {
            string id = txtID.Text;
            string password = txtPassword.Password;

            if (IsValidUser(id, password))
            {
                await this.ShowMessageAsync("로그인", "로그인 성공!");

                // MainPage로 이동
                MainPage mainPage = new MainPage();

                // 메인 윈도우 생성
                MainWindow mainWindow = new MainWindow();

                // 메인 페이지로 네비게이트
                mainWindow.MainFrame.Navigate(mainPage);

                // 현재 창 닫기
                this.Close();

                // 메인 윈도우 보이기
                mainWindow.Show();
            }
            else
            {
                await this.ShowMessageAsync("로그인", "로그인 실패!");
            }
        }

        private bool IsValidUser(string id, string password)
        {
            return id == "asdf" && password == "1234";
        }

        private void txtID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtID.Text == "ID")
            {
                txtID.Text = "";
                txtID.Foreground = Brushes.Black;
            }
        }

        private void txtID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                txtID.Text = "ID";
                txtID.Foreground = Brushes.LightGray;
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox.Password == "Password")
            {
                passwordBox.Password = "";
                passwordBox.Foreground = Brushes.Black;
            }
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBox.Password = "Password";
                passwordBox.Foreground = Brushes.LightGray;
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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
    }
}
