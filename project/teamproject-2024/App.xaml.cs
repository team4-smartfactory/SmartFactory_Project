using System.Windows;

namespace teamproject_2024
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 시작 윈도우를 LoginPage로 설정
            LoginPage loginPage = new LoginPage();
            loginPage.Show();

            // 로그인 성공 후 메인 윈도우 표시
            if (loginPage.DialogResult == true) // 로그인 성공 시 true로 설정된 DialogResult 확인
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowState = WindowState.Maximized; // 최대화로 설정
                mainWindow.Show();
            }
        }
    }
}
