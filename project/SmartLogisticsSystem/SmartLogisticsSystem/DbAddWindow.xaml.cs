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
using System.IO.Ports;
using System.IO;

namespace SmartLogisticsSystem
{
    /// <summary>
    // <주의사항>아두이노 IDE와 C#의 프로그램이 같은 포트를 쓸 경우 프로그램 충돌이 난다.여러가지 해결 방법이 있겠지만,
    // 그냥 C# 프로그램을 실행시킬 때는 아두이노 IDE를 잠시 꺼주면 되겠다.
    /// </summary>
    public partial class DbAddWindow : Window
    {
        SerialPort port = new SerialPort();
        public DbAddWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {

            port.Write("go");

        }
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            port.Write("stop");

        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                port.PortName = "COM3";
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;

                // 포트 열기 전 디버깅 메시지 추가
                MessageBox.Show("포트를 엽니다");
                port.Open();

                // 포트를 연 후 디버깅 메시지 추가
                MessageBox.Show("포트를 열었습니다");

                port.DiscardInBuffer();
                if (port.IsOpen)
                {
                    MessageBox.Show("포트가 연결되었습니다");

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생했습니다: " + ex.Message);
            }
        }

    }
}
