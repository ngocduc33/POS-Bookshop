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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyShop.Custom;
using LiveCharts;
using LiveCharts.Wpf;
using SeriesCollection = LiveCharts.SeriesCollection;
using MyShop.Report;
using System.Windows.Media.Animation;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for ReportScreen.xaml
    /// </summary>
    public partial class ReportScreen : UserControl
    {
        public ReportScreen()
        {
            InitializeComponent();
        }

        int vh = 2252000;
        int kt = 469000;
        int tlkns = 513000;
        int tn = 83000;
        int ndc = 562000;
        int hnn = 326000;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            var series = new SeriesCollection()
            {
                new PieSeries()
                {
                    Title="Văn học",
                    Values= new ChartValues<int>{vh},
                    DataLabels = true
                },
                new PieSeries()
                {
                    Title="Kinh tế",
                    Values= new ChartValues<int>{kt},
                    DataLabels = true
                },
                new PieSeries()
                {
                    Title="Tâm lý - kỹ năng sống",
                    Values= new ChartValues<int>{tlkns},
                    DataLabels = true
                },
                new PieSeries()
                {
                    Title="Thiếu nhi",
                    Values= new ChartValues<int>{tn},
                    DataLabels = true
                },
                new PieSeries()
                {
                    Title="Nuôi dạy con",
                    Values= new ChartValues<int>{ndc},
                    DataLabels = true
                },

                new PieSeries()
                {
                    Title="Học ngoại ngữ",
                    Values= new ChartValues<int>{hnn},
                    DataLabels = true
                }
            };
            reportChart.Series = series;
        }

        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            reportScreen.Children.Clear();//Quay lại màn hình Dashboard
        }

        private void MasterDataReport_Click(object sender, RoutedEventArgs e)
        {
            reportScreen.Children.Add(new masterDataReport());
        }

        private void BestSeller_Click(object sender, RoutedEventArgs e)
        {
            reportScreen.Children.Add(new bestseller());
        }

        private void fromDayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void toDayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var db = new MyShopEntities();
            
            var orders = db.OrderDetails;
            var products = db.Products;
            var orderdetails = db.OrderDetails;

            if (fromDayDatePicker.SelectedDate != null)
            {
                var from = (DateTime)fromDayDatePicker.SelectedDate;
                var to = (DateTime)toDayDatePicker.SelectedDate;

                if (DateTime.Compare(from, to) <=0 )
                {
                    var query = from orderdetail in orderdetails
                                join p in products on orderdetail.ProductId equals p.Product_Id
                                group p by p.CatId into pGroup
                                select new
                                {
                                    Sum = orderdetails.Sum(o=>o.Total)
                                };
                    
                }
                else
                {
                    var dialogError = new Messenge() { Message = "The end date must be greater than the start date." };
                    dialogError.Sounds();
                    dialogError.Owner = Window.GetWindow(this);
                    dialogError.ShowDialog();
                    return;
                }
            }
            else
            {
                var dialogError = new Messenge() { Message = "Please select a start date." };
                dialogError.Sounds();
                dialogError.Owner = Window.GetWindow(this);
                dialogError.ShowDialog();
                return;
            }
        }

       
    }

    
}
