using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace teamproject_2024
{
    /// <summary>
    /// GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class GraphPage : Page
    {
        public ViewModel ViewModel { get; private set; }
        public GraphPage()
        {
            InitializeComponent();
            ViewModel = new ViewModel();
            DataContext = ViewModel;

            // 데이터 가져오고 차트 업데이트
            ViewModel.get_data_graph();
        }
    }

    public class ViewModel
    {
        public int red { get; private set; }
        public int green { get; private set; }
        public int blue { get; private set; }

        public ISeries[] Series { get; private set; }

        public ViewModel()
        {
            // 초기 Series를 빈 배열로 설정
            Series = Array.Empty<ISeries>();
        }

        public void get_data_graph()
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    using (var Rcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_RED, conn))
                    {
                        var R_result = Rcount.ExecuteScalar();
                        red = R_result != null ? Convert.ToInt32(R_result) : 0;
                    }

                    using (var Gcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_GREEN, conn))
                    {
                        var G_result = Gcount.ExecuteScalar();
                        green = G_result != null ? Convert.ToInt32(G_result) : 0;
                    }

                    using (var Bcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_BLUE, conn))
                    {
                        var B_result = Bcount.ExecuteScalar();
                        blue = B_result != null ? Convert.ToInt32(B_result) : 0;
                    }

                    // Series 속성 업데이트 및 색상 설정
                    Series = new ISeries[]
                    {
                        new PieSeries<double>
                        {
                            Values = new double[] { red },
                            Fill = new SolidColorPaint(SKColors.Red) // 빨간색
                        },
                        new PieSeries<double>
                        {
                            Values = new double[] { green },
                            Fill = new SolidColorPaint(SKColors.Green) // 초록색
                        },
                        new PieSeries<double>
                        {
                            Values = new double[] { blue },
                            Fill = new SolidColorPaint(SKColors.Blue) // 파란색
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }
    }
}
