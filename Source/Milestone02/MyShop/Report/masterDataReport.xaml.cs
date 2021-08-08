using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for masterDataReport.xaml
    /// </summary>
    public partial class masterDataReport : UserControl
    {
        PagingInfo _pagingInfo;
        int rowsPerPage = 10;
        int quantity = int.MaxValue;


        public masterDataReport()
        {
            InitializeComponent();
        }

        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            masterReport.Children.Clear();
        }

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            statusLabel.Content = "Application is ready";

            var db = new MyShopEntities();

            await Task.Run(() =>
            {
                Thread.Sleep(1000);
            });
            categoriesComboBox.ItemsSource = db.Categories.ToList();
            categoriesComboBox.SelectedIndex = 0;
        }

        #region Xử lý hiệu ứng Comboxbox
        /// <summary>
        /// Hiệu ứng khi chọn
        /// </summary>
        private void ComboProductTypes_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.LightGray;
        }

        /// <summary>
        /// Hiệu ứng khi bỏ chọn
        /// </summary>
        private void ComboProductTypes_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.Transparent;
        }

        #endregion

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePagingInfo();
            UpdateProductView();
        }

        public void CalculatePagingInfo(MyShopEntities db, Category selectedCategory)
        {

            ICollection<Product> products = db.Categories.Find(selectedCategory.Category_Id).Products;
            var query = from product in products
                        where product.Quantity<quantity
                        select product;

            var count = query.Count();

            // Tinh toan thong tin phan trang
            _pagingInfo = new PagingInfo()
            {
                RowsPerPage = rowsPerPage,
                TotalItems = count,
                TotalPages = count / rowsPerPage +
                    (((count % rowsPerPage) == 0) ? 0 : 1),
                CurrentPage = 1
            };

            comboBoxPaging.ItemsSource = _pagingInfo.Pages;
            comboBoxPaging.SelectedIndex = 0;

            statusLabel.Content = $"Tổng sản phẩm: {count} ";
        }


        private void PreviousPaging_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = comboBoxPaging.SelectedIndex;
            if (currentIndex > 0)
            {
                comboBoxPaging.SelectedIndex--;
            }
        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int nextPage = comboBoxPaging.SelectedIndex + 1;
            _pagingInfo.CurrentPage = nextPage;

            UpdateProductView();
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CalculatePagingInfo();
            //UpdateProductView();
        }

        private void NextPaging_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = comboBoxPaging.SelectedIndex;
            if (currentIndex <= _pagingInfo.Pages.Count)
            {
                comboBoxPaging.SelectedIndex++;
            }
        }

        private void status_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var db = new MyShopEntities();
            var selectedCategory = categoriesComboBox.SelectedItem as Category;

            if (status_Checkbox.IsChecked == true)
            {
                quantity = 10;
            }
            
            CalculatePagingInfo(db, selectedCategory);
            UpdateProductView(db, selectedCategory);
        }
        private void status_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            var db = new MyShopEntities();
            var selectedCategory = categoriesComboBox.SelectedItem as Category;
            quantity = int.MaxValue;

            CalculatePagingInfo(db, selectedCategory);
            UpdateProductView(db, selectedCategory);
        }
        public void UpdateProductView(MyShopEntities db, Category selectedCategory)
        {
            ICollection<Product> products = db.Categories.Find(selectedCategory.Category_Id).Products;
            var query = (from product in products
                         where product.Quantity<quantity
                         select new
                         {
                             product.Product_Id,
                             ProductName = product.Product_Name,
                             Thumbnail = product.Photos.First().Data,
                             product.Price,
                             product.Quantity
                         });
            var skip = (_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage;
            var take = _pagingInfo.RowsPerPage;
            productsListView.ItemsSource = query.Skip(skip).Take(take);
        }

        void CalculatePagingInfo()
        {
            var db = new MyShopEntities();
            var _selectedCategoryIndex = categoriesComboBox.SelectedItem as Category;
            var products = db.Categories.Find(_selectedCategoryIndex.Category_Id).Products;
            
            var query = from product in products
                        
                        select product.Product_Id;

            var count = query.Count();
            _pagingInfo = new PagingInfo()
            {
                RowsPerPage = rowsPerPage,
                TotalItems = count,
                TotalPages = count / rowsPerPage +
                    (((count % rowsPerPage) == 0) ? 0 : 1),
                CurrentPage = 1
            };

            comboBoxPaging.ItemsSource = _pagingInfo.Pages;
            comboBoxPaging.SelectedIndex = 0;

            statusLabel.Content = $"Tổng sản phẩm: {count} ";

        }

        void UpdateProductView()
        {
            var db = new MyShopEntities();
            var _selectedCategoryIndex = categoriesComboBox.SelectedItem as Category;
            var products = db.Categories.Find(_selectedCategoryIndex.Category_Id).Products;
            
            var query = from product in products
                        where product.Quantity < quantity
                            select new
                            {
                                product.Product_Id,
                                ProductName = product.Product_Name,
                                Thumbnail = product.Photos.First().Data,
                                product.Price,
                                product.Quantity
                            };
            
            // Gan du lieu cho list view de o cuoi cung
            // Dua theo trang hien tai
            var skip = (_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage;
            var take = _pagingInfo.RowsPerPage;
            productsListView.ItemsSource = query.Skip(skip).Take(take);
        }

        public void refresh(bool Data)
        {
            if (!Data) return;

            CalculatePagingInfo();
            UpdateProductView();
        }

        
    }
}
