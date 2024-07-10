using System;
using System.IO.Ports;
using System.Windows;

namespace SmartLogisticsSystem
{
    public partial class DbAddWindow : Window
    {
        SerialPort port;
        string color;
        string box_color;
        string received_Data;
        public DbAddWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // SerialPort 객체를 초기화합니다.
            port = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.None
            };

            // SerialPort의 DataReceived 이벤트 핸들러를 추가합니다.
            port.DataReceived += Arduino_DataReceived;
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Write("go");
            }
            else
            {
                MessageBox.Show("포트가 열려있지 않습니다.");
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Write("stop");
            }
            else
            {
                MessageBox.Show("포트가 열려있지 않습니다.");
            }
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!port.IsOpen)
                {
                    port.Open();
                    MessageBox.Show("포트를 열었습니다");
                }
                else
                {
                    MessageBox.Show("포트가 이미 열려있습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생했습니다: " + ex.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    MessageBox.Show("종료되었습니다");
                }
                else
                {
                    MessageBox.Show("포트가 열려있지 않습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생했습니다: " + ex.Message);
            }
        }

        private void Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                received_Data = port.ReadLine();
                // 비동기적으로 UI를 업데이트합니다.
                Dispatcher.Invoke(() => {
                    //MessageBox.Show($"{received_Data}");
                    string[] result = received_Data.Split(',');
                    color = result[0];
                    box_color = result[1];
                    MessageBox.Show($"{color} + {box_color}");
                });
            }
            catch (Exception ex)
            {
                // 예외 처리 (필요한 경우)
                MessageBox.Show("데이터 수신 중 오류가 발생했습니다: " + ex.Message);
            }
        }
    }
}
