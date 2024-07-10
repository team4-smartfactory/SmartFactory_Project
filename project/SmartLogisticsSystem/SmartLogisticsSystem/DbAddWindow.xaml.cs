using System;
using System.IO.Ports;
using System.Windows;
using SmartLogisticsSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // SerialPort 객체를 초기화합니다.
            port = new SerialPort
            {
                PortName = "COM4",
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
            Select_Data();
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
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.SmartLogistics.INSERT_QUERY, conn);
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
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var cmd = new SqlCommand(Models.SmartLogistics.SELECT_QUERY, conn);
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
                    DgvResult.ItemsSource = smartLogistics;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }
    }
}
