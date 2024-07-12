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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

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

        private async void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Write("go");
            }
            else
            {
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "포트", "포트가 열려있지 않습니다");
            }
        }

        private async void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Write("stop");
            }
            else
            {
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "포트", "포트가 열려있지 않습니다");
            }
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                port.PortName = "COM3";
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;
                // SerialPort의 DataReceived 이벤트 핸들러를 추가합니다.
                port.DataReceived += Arduino_DataReceived;

                // 포트 열기 전 디버깅 메시지 추가
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "포트", "포트를 엽니다");
                port.Open();

                // 포트를 연 후 디버깅 메시지 추가
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "포트", "포트를 열었습니다");

                port.DiscardInBuffer();
                if (port.IsOpen)
                {
                    await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "포트", "포트가 연결되었습니다");

                }
            }

            catch (Exception ex)
            {
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "오류", "오류가 발생했습니다: " + ex.Message);
            }
            Select_Data();
        }

        private async void Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
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
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "오류", "데이터 수신 중 오류가 발생했습니다: " + ex.Message);
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

        private async void Select_Data()
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
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "오류", "데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }


        private void ProTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (ProTextBox != null && textBox.Text == "제품명 입력")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.Black); // 사용자 입력시 텍스트 색상
            }
        }

        private void ProTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetWatermark(textBox, "제품명 입력");
            }
        }
        private void SetWatermark(TextBox textBox, string watermark)
        {
            textBox.Text = watermark;
            textBox.Foreground = new SolidColorBrush(Colors.Gray); // 워터마크 텍스트 색상
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.INSERT_QUERY, conn);

                    var selectedDiviItem = DiviComboBox.SelectedItem as ComboBoxItem;
                    string division = selectedDiviItem?.Content.ToString();
                    SqlParameter prmDivi = new SqlParameter("@Division", division);
                    cmd.Parameters.Add(prmDivi);
                    SqlParameter prmProduct = new SqlParameter("@Product", ProTextBox.Text);
                    cmd.Parameters.Add(prmProduct);
                    SqlParameter prmDate = new SqlParameter("@Date", DatePicker.SelectedDateTime);
                    cmd.Parameters.Add(prmDate);

                    var result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "저장", "저장성공!");
                    }
                    else
                    {
                        await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "실패", "저장실패!");
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogManager.ShowMessageAsync(Window.GetWindow(this) as MetroWindow, "오류" ,"오류 발생" + ex.Message);
            }
            Select_Data();
        }
    }
}
