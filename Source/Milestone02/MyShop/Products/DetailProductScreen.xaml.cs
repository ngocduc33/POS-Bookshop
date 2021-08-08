using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MyShop.Products
{
    /// <summary>
    /// Interaction logic for DetailProductScreen.xaml
    /// </summary>
    public partial class DetailProductScreen : UserControl
    {
        // Delegate để refresh danh sách sản phẩm
        public delegate void DelegateRefeshProductList(bool Data);
        public DelegateRefeshProductList RefreshProductList;

        Product _product;
        public DetailProductScreen(Product product)
        {
            InitializeComponent();

            _product = product;

            producIDTextBox.Text = _product.Product_Id.ToString();
            productNameTextBox.Text = _product.Product_Name;
            storeCodeTextBox.Text = _product.SKU;
            productPriceTextbox.Text = _product.Price.ToString();
            productQuantityTextbox.Text = _product.Quantity.ToString();
            if (_product.Description != null) productDescriptionTextBox.Text = _product.Description;
            producCategoryTextBox.Text = _product.Category.Category_Name;

            imgProduct.Source = LoadImage(_product.Photos.First().Data);
        }

        /// <summary>
        /// load hình ảnh với dãy byte[]
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        /// <summary>
        /// Quay lại product view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            detailProduct.Children.Clear();
        }
        /// <summary>
        /// Định giá cả
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Chuyển định dạng abc,xyz cho giá cả
            if (textBox.Text.Length > 0)
            {
                double value = 0;
                double.TryParse(textBox.Text, out value);
                textBox.Text = value.ToString("N0");
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }


        /// <summary>
        /// Click chuyển sang màn hình add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (RefreshProductList != null)
            {
                RefreshProductList.Invoke(true);
            }

            var screenEdit = new NewProductScreen(_product);
            screenEdit.RefreshProductList = refresh;
            detailProduct.Children.Add(screenEdit);
        }

        /// <summary>
        /// xóa sản phẩm khỏi database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            // Hiện thông báo xác nhận
            var dialog = new Dialog() { Message = "Delete this product, Are you sure?" };
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == false) return;

            //thao tác dưới cơ sở dữ liệu
            var db = new MyShopEntities();

            try
            {
                //xóa product sẽ xóa luôn ảnh của product

                //Tìm hình ảnh của product để xóa
                var imageProductFromDatabase = db.Photos.Find(_product.Product_Id);
                db.Photos.Remove(imageProductFromDatabase);
                db.SaveChanges();

                //tìm product theo id để xóa
                var productFromDatabase = db.Products.Find(_product.Product_Id);
                db.Products.Remove(productFromDatabase);
                db.SaveChanges();//cập nhật

                // cập nhật giao diện
                if (RefreshProductList != null)
                {
                    RefreshProductList.Invoke(true);
                    //tắt nút edit
                    btnEditProduct.IsEnabled = false;
                    //gán content = deleted và tắt nút false
                    btnDeleteProduct.Content = "Deleted";
                   
                    btnDeleteProduct.IsEnabled = false;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Làm mới thông tin
        /// </summary>
        public void refresh(bool Data)
        {
            var image = _product.Photos.First().Data;
            if (Data) // Nếu vừa sửa xong
            {
                // Lấy lại đối tượng mới
                var db = new MyShopEntities();
                _product = db.Products.Find(_product.Product_Id);

                // Làm mới danh sách ở trang trước (delegate 2 cấp)
                if (RefreshProductList != null)
                {
                    RefreshProductList.Invoke(true);
                }
            }

            // Đưa thông tin lên UI
            productNameTextBox.Text = _product.Product_Name;
            //producIDTextBox.Text = _product.Product_Id;
            productPriceTextbox.Text = _product.Price.ToString();
            // editProductType.Text = product.ProductType;
            if (_product.Description != null) productDescriptionTextBox.Text = _product.Description;
            productQuantityTextbox.Text = _product.Quantity.ToString();

            //image
            if (image != null)
            {
                imgProduct.Source = LoadImage(_product.Photos.First().Data);
            }

            producCategoryTextBox.Text = _product.Category.Category_Name;
        }
    }
}
