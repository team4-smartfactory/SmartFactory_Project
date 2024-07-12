using SmartLogisticsSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace teamproject_2024
{
    /// <summary>
    /// Inventory.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InventoryPage : Page
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        // 실시간조회 버튼 클릭
        private void BtnReq_Click(object sender, RoutedEventArgs e)
        {
            List<SmartLogistics> smartLogistics = new List<SmartLogistics>();
            IdTextBox.Clear();
            ProdTextBox.Clear();
            RedCheck.IsChecked = false;
            BlueCheck.IsChecked = false;
            GreenCheck.IsChecked = false;

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
                    DgvResult.ItemsSource = smartLogistics;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgvResult.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 품목을 선택하세요");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var delRes = 0;

                    foreach (SmartLogistics item in DgvResult.SelectedItems)
                    {
                        SqlCommand cmd = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.DELETE_QUERY, conn);
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

            BtnReq_Click(sender, e);
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;
                    SqlCommand cmd = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.UPDATE_QUERY, conn);

                    SqlParameter prmProduct = new SqlParameter("@Product", ProdTextBox.Text);
                    cmd.Parameters.Add(prmProduct);
                    string divisionValue = string.Empty;
                    if (RedCheck.IsChecked == true)
                    {
                        divisionValue = "RED";
                    }
                    else if (BlueCheck.IsChecked == true)
                    {
                        divisionValue = "BLUE";
                    }
                    else if (GreenCheck.IsChecked == true)
                    {
                        divisionValue = "GREEN";
                    }
                    SqlParameter prmDivision = new SqlParameter("@Division", divisionValue);
                    cmd.Parameters.Add(prmDivision); ;

                    //TODO 날짜 변경은 되는데 시간 변경이 안됨 처리 해야함.
                    SqlParameter prmDate = new SqlParameter("@Date", DatePicker.SelectedDateTime);
                    cmd.Parameters.Add(prmDate);
                    SqlParameter prmId = new SqlParameter("@Id", IdTextBox.Text);
                    cmd.Parameters.Add(prmId);

                    insRes += cmd.ExecuteNonQuery();

                    if (insRes > 0)
                    {
                        // this 메시지박스의 부모창이 누구냐, FrmLoginUser
                        MessageBox.Show("저장");
                        //MessageBox.Show("저장성공!", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("저장");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            BtnReq_Click(sender, e);
        }
        private void DgvResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DgvResult.SelectedItem is SmartLogistics selectedSmartLogistics)
            {
                IdTextBox.Text = selectedSmartLogistics.Id.ToString();
                ProdTextBox.Text = selectedSmartLogistics.Product.ToString();
                DatePicker.SelectedDateTime = selectedSmartLogistics.Date.Date;
                if (selectedSmartLogistics.Division.ToString() == "RED")
                {
                    RedCheck.IsChecked = true;
                    BlueCheck.IsChecked = false;
                    GreenCheck.IsChecked = false;
                }
                else if (selectedSmartLogistics.Division.ToString() == "BLUE")
                {
                    RedCheck.IsChecked = false;
                    BlueCheck.IsChecked = true;
                    GreenCheck.IsChecked = false;
                }
                else if (selectedSmartLogistics.Division.ToString() == "GREEN")
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


        private void CheckBox_checked(object sender, RoutedEventArgs e)
        {
            CheckBox clickedCheckBox = sender as CheckBox;

            // 다른 체크박스들의 상태를 조정하여 하나만 선택되도록 만듭니다.
            if (clickedCheckBox == RedCheck && clickedCheckBox.IsChecked == true)
            {
                BlueCheck.IsChecked = false;
                GreenCheck.IsChecked = false;
            }
            else if (clickedCheckBox == BlueCheck && clickedCheckBox.IsChecked == true)
            {
                RedCheck.IsChecked = false;
                GreenCheck.IsChecked = false;
            }
            else if (clickedCheckBox == GreenCheck && clickedCheckBox.IsChecked == true)
            {
                RedCheck.IsChecked = false;
                BlueCheck.IsChecked = false;
            }
        }
    }
}
