using Microsoft.Data.SqlClient;
using SmartLogisticsSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
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

        // 실시간조회 버튼 클릭
        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DgvResult.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 품목을 선택하세요");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var delRes = 0;

                    foreach (SmartLogistics item in DgvResult.SelectedItems)
                    {
                        SqlCommand cmd = new SqlCommand(Models.SmartLogistics.DELETE_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Id", item.Id);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == DgvResult.SelectedItems.Count)
                    {
                        MessageBox.Show($"{delRes}건 삭제");
                    }
                    else
                    {
                        MessageBox.Show($"{DgvResult.SelectedItems.Count}건중 {delRes}건 삭제");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"삭제 오류 {ex.Message}");
            }

            Button_Click(sender, e);
        }

        private void DgvResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DgvResult.SelectedItem is SmartLogistics selectedSmartLogistics)
            {
                IdTextBox.Text = selectedSmartLogistics.Id.ToString();
                ProdTextBox.Text = selectedSmartLogistics.Product.ToString();
                DatePicker.SelectedDateTime = selectedSmartLogistics.Date.Date;
                if (selectedSmartLogistics.Division.ToString() == "레드")
                {
                    RedCheck.IsChecked = true;
                    BlueCheck.IsChecked = false;
                    GreenCheck.IsChecked = false;
                }
                else if (selectedSmartLogistics.Division.ToString() == "블루")
                {
                    RedCheck.IsChecked = false;
                    BlueCheck.IsChecked = true;
                    GreenCheck.IsChecked = false;
                }
                else if (selectedSmartLogistics.Division.ToString() == "그린")
                {
                    RedCheck.IsChecked = false;
                    BlueCheck.IsChecked = false;
                    GreenCheck.IsChecked = true;
                }
                else // 다른 제품이라면 모든 체크를 해제
                {
                    RedCheck.IsChecked = false;
                    BlueCheck.IsChecked = false;
                    GreenCheck.IsChecked = false;
                }
            }
        }
    }
}
