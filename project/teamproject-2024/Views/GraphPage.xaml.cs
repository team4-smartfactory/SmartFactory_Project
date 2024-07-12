using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace teamproject_2024
{
    /// <summary>
    /// GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GraphPage : Page
    {
        public ViewModel ViewModel { get; private set; }
        public GraphPage()
        {
            InitializeComponent();
            ViewModel = new ViewModel();
            DataContext = ViewModel;

            // 데이터 가져오고 차트 업데이트
            ViewModel.GetDataGraph();
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private int _red;
        private int _green;
        private int _blue;
        private ISeries[] _series;

        public int Red
        {
            get => _red;
            set
            {
                if (_red != value)
                {
                    _red = value;
                    OnPropertyChanged();
                    UpdateSeries();
                }
            }
        }

        public int Green
        {
            get => _green;
            set
            {
                if (_green != value)
                {
                    _green = value;
                    OnPropertyChanged();
                    UpdateSeries();
                }
            }
        }

        public int Blue
        {
            get => _blue;
            set
            {
                if (_blue != value)
                {
                    _blue = value;
                    OnPropertyChanged();
                    UpdateSeries();
                }
            }
        }

        public ISeries[] Series
        {
            get => _series;
            set
            {
                if (_series != value)
                {
                    _series = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModel()
        {
            Series = Array.Empty<ISeries>();
        }

        public void GetDataGraph()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SmartLogisticsSystem.Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    using (var Rcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_RED, conn))
                    {
                        var R_result = Rcount.ExecuteScalar();
                        Red = R_result != null ? Convert.ToInt32(R_result) : 0;
                    }

                    using (var Gcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_GREEN, conn))
                    {
                        var G_result = Gcount.ExecuteScalar();
                        Green = G_result != null ? Convert.ToInt32(G_result) : 0;
                    }

                    using (var Bcount = new SqlCommand(SmartLogisticsSystem.Models.SmartLogistics.SELECT_QUERY_BLUE, conn))
                    {
                        var B_result = Bcount.ExecuteScalar();
                        Blue = B_result != null ? Convert.ToInt32(B_result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void UpdateSeries()
        {
            Series = new ISeries[]
            {
                new PieSeries<double>
                {
                    Values = new double[] { Red },
                    Fill = new SolidColorPaint(SKColors.Red)
                },
                new PieSeries<double>
                {
                    Values = new double[] { Green },
                    Fill = new SolidColorPaint(SKColors.Green)
                },
                new PieSeries<double>
                {
                    Values = new double[] { Blue },
                    Fill = new SolidColorPaint(SKColors.Blue)
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
