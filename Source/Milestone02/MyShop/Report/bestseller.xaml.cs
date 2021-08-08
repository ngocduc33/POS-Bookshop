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

namespace MyShop.Report
{
    /// <summary>
    /// Interaction logic for bestseller.xaml
    /// </summary>
    public partial class bestseller : UserControl
    {
        public bestseller()
        {
            InitializeComponent();
        }
        PagingInfo _pagingInfo;
        int rowsPerPage = 10;

        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            bestsellerData.Children.Clear();//Quay lại màn hình Dashboard
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProductView();
        }


        void UpdateProductView()
        {
            var db = new MyShopEntities();
            var products = db.Products;
            var orderdetails = db.OrderDetails;

            var query =
            from orderdetail in orderdetails
            group orderdetail by orderdetail.ProductId into orderGroup
            join p in products on orderGroup.Key equals p.Product_Id
            orderby orderGroup.Sum(o => o.Quantity) descending
            select new
            {
                ProductName = p.Product_Name,
                Thumbnail = p.Photos.FirstOrDefault().Data,
                Price = p.Price,
                Count = orderGroup.Sum(o=>o.Quantity),

            };


            // Gan du lieu cho list view de o cuoi cung
            // Dua theo trang hien tai
            var take = 7;
            productsListView.ItemsSource = query.Take(take).ToList();
        }

    }
}
