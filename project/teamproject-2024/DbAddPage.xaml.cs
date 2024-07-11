using SmartLogisticsSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
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
using Microsoft.Data.SqlClient;

namespace teamproject_2024
{
    /// <summary>
    /// DbAddWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DbAddPage : Page
    {
        SerialPort port = new SerialPort();
        string color;
        string box_color;
        string received_Data;
        public DbAddPage()
        {
            InitializeComponent();
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
                port.PortName = "COM5";
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;
                // SerialPort의 DataReceived 이벤트 핸들러를 추가합니다.
                port.DataReceived += Arduino_DataReceived;

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
            Select_Data();
        }

        private void Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                received_Data = port.ReadLine();
                // 비동기적으로 UI를 업데이트합니다.
                Dispatcher.Invoke(() => {
                    if (received_Data.LastIndexOf('?') > 0)
                    {
                        return;
                    }
                    //MessageBox.Show($"{received_Data}");

                    string[] result = received_Data.Split(',');
                    color = result[0];
                    box_color = result[1];
                    Debug.WriteLine($"{color} + {box_color}");

                    DateTime day = DateTime.Now;
                    Insert_Data(color, box_color, day);
                });
            }
            catch (Exception ex)
            {
                // 예외 처리 (필요한 경우)
                MessageBox.Show("데이터 수신 중 오류가 발생했습니다: " + ex.Message);
            }

        }
        private void Insert_Data(string COL, string COLBOX, DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.INSERT_QUERY, conn);
                cmd.Parameters.AddWithValue("@Division", COL);
                cmd.Parameters.AddWithValue("@Product", COLBOX);
                cmd.Parameters.AddWithValue("@Date", date);

                cmd.ExecuteNonQuery(); // 이게 없으니 안들어가지~
            }

            Select_Data();
        }

        private void Select_Data()
        {
            List<SmartLogistics> smartLogistics = new List<SmartLogistics>();

            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var cmd = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "SmartLogistics");

                    foreach (DataRow row in dSet.Tables["SmartLogistics"].Rows)
                    {
                        var smartlogistcs = new SmartLogistics()
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Division = Convert.ToString(row["Division"]),
                            Product = Convert.ToString(row["Product"]),
                            Date = Convert.ToDateTime(row["Date"]),
                        };

                        smartLogistics.Add(smartlogistcs);
                    }
                    GrdResults.ItemsSource = smartLogistics;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }
    }
}
