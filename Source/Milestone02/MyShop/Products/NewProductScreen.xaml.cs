using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using MyShop.Categories;

namespace MyShop.Products
{
    /// <summary>
    /// Interaction logic for NewProductScreen.xaml
    /// </summary>
    public partial class NewProductScreen : UserControl
    {
        //cờ xác định sửa hay thêm;
        public bool isEditProduct = false;

        // Delegate để refresh danh sách sản phẩm
        public delegate void DelegateRefeshProductList(bool Data);
        public DelegateRefeshProductList RefreshProductList;
        public NewProductScreen()
        {
            InitializeComponent();

            //Lấy và hiển thị danh sách sản phẩm
            Thread catThread = new Thread(delegate ()
            {
                var db = new MyShopEntities();
                var category = new ObservableCollection<Category>(db.Categories.ToList());
                Dispatcher.Invoke(() => {
                    producIDTextBox.Text = "Auto";
                    producIDTextBox.IsEnabled = false;

                    ProductTypeComboxBox.ItemsSource = category;
                });
            });
            catThread.Start();
        }
        public NewProductScreen(Product product)
        {
            InitializeComponent();

            isEditProduct = true;

            var image = product.Photos.First().Data;

            //Thông tin UI
            Title.Content = "EDIT PRODUCT";
            producIDTextBox.Text = product.Product_Id.ToString();//id
            producIDTextBox.IsEnabled = false;// tắt sửa id
            productNameTextBox.Text = product.Product_Name; //tên sản phẩm
            storeCodeTextBox.Text = product.SKU; // mã kho
            productPriceTextbox.Text = product.Price.ToString(); //giá
            productQuantityTextbox.Text = product.Quantity.ToString(); //số lượng

            //Kiểm tra mô tả
            if (product.Description != null) 
                productDescriptionTextBox.Text = product.Description;

            //image
            imgProduct.Source = LoadImage(product.Photos.First().Data);
            imgProduct.Tag = LoadImage(product.Photos.First().Data);

            Thread catThread = new Thread(delegate ()
            {
                var db = new MyShopEntities();
                var _catID = db.Categories.Find(product.CatId);
                var category = new ObservableCollection<Category>(db.Categories.ToList());

                Dispatcher.Invoke(() => {
                    ProductTypeComboxBox.ItemsSource = category;

                    //tìm loại sản phẩm tương ứng
                    for(int i = 0; i < category.Count; i++)
                    {
                        if (category[i].Category_Id.Equals(_catID.Category_Id))
                        {
                            ProductTypeComboxBox.SelectedIndex = i; break;
                        }
                    }
                });
            });
            catThread.Start();
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
        /// <summary>
        /// xử lý ô giá, nhận giá trị số
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        #region Xử lý hiệu ứng Comboxbox
        /// <summary>
        /// Hiệu ứng khi chọn
        /// </summary>
        private void ComboProductTypes_DropDownOpened(object sender, EventArgs e)
        {
            ProductTypeComboxBox.Background = Brushes.LightGray;
        }

        /// <summary>
        /// Hiệu ứng khi bỏ chọn
        /// </summary>
        private void ComboProductTypes_DropDownClosed(object sender, EventArgs e)
        {
            ProductTypeComboxBox.Background = Brushes.Transparent;
        }
        #endregion

        /// <summary>
        /// Quay lại Product View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            newProduct.Children.Clear();
        }

        private void categoriesComboBox_Click(object sender, RoutedEventArgs e)
        {
            var categories = new CategoryScreen();
            categories.RefreshCategoriesList = RefreshComboBox;
            newProduct.Children.Add(categories);

        }

        private void BtnAddImageProduct_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (true == openFileDialog.ShowDialog())
            {
                string filename = openFileDialog.FileName;
                try
                {
                    BitmapImage source = new BitmapImage(new Uri(filename));
                    imgProduct.Source = source;
                    imgProduct.Tag = filename;
                }
                catch (Exception ex)
                {
                    var dialogError = new Dialog() { Message = "Tập tin ảnh không hợp lệ" };
                    dialogError.Owner = Window.GetWindow(this);
                    dialogError.ShowDialog();
                    return;
                }
            }
        }

        private void BtnRefesh_Click(object sender, RoutedEventArgs e)
        {
            productNameTextBox.Clear();
            storeCodeTextBox.Clear();
            productPriceTextbox.Clear();
            productQuantityTextbox.Clear();
            ProductTypeComboxBox.SelectedIndex = -1;

            var img = new BitmapImage();

            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,/Images/Icon/defaultImage.png");
            img.EndInit();

            imgProduct.Source = img;
            imgProduct.Tag = null;

        }

        private void BtnAddProductSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra thông tin có đầy đủ
            if (producIDTextBox.Text.Length == 0
                ||productNameTextBox.Text.Length == 0
                || productPriceTextbox.Text.Length == 0
                || productQuantityTextbox.Text.Length == 0
                || productDescriptionTextBox.Text.Length == 0
                || ProductTypeComboxBox.SelectedIndex == -1)
            {
                var dialogError1 = new Dialog() { Message = "Please enter all the information!" };
                dialogError1.Owner = Window.GetWindow(this);
                dialogError1.ShowDialog();
                return;
            }

            var dialogError = new Dialog() { Message = isEditProduct ? "Confirm product repair, Are you sure?" : "Add new product, Are you sure?" };
            dialogError.Owner = Window.GetWindow(this);

            if (true == dialogError.ShowDialog())
            {
                try
                {
                    var db = new MyShopEntities();

                    // Tạo đối tượng Product tương ứng
                    var product = new Product()
                    {
                        //Product_Id = int.Parse(producIDTextBox.Text),
                        Product_Name = productNameTextBox.Text,
                        SKU = storeCodeTextBox.Text,
                        Price = decimal.Parse(productPriceTextbox.Text),
                        Quantity = int.Parse(productQuantityTextbox.Text),
                        Description = productDescriptionTextBox.Text.Length == 0 ? null : productDescriptionTextBox.Text,
                        
                        // Còn thiếu trường CatId và photo
                    };
                    // Tìm Id của loại sản phẩm đã chọn
                    var category = ProductTypeComboxBox.SelectedValue as Category;

                    if (category != null)
                    {
                        product.CatId = category.Category_Id;

                        // Nếu sửa
                        if (isEditProduct)
                        {
                            try
                            {
                                var oldProduct = db.Products.Find(int.Parse(producIDTextBox.Text));

                                oldProduct.Product_Name = product.Product_Name;
                                oldProduct.SKU = product.SKU;
                                oldProduct.Price = product.Price;
                                oldProduct.Description = product.Description;
                                oldProduct.Quantity = product.Quantity; // số lượng mới

                                //thiếu catID và photo

                                if (oldProduct.CatId != category.Category_Id) // Nếu có thay đổi mã sản phẩm
                                {
                                    oldProduct.CatId = product.CatId;
                                }


                                //trường image
                                if (imgProduct.Tag.ToString() != null)
                                {
                                    var imageFull = imgProduct.Tag.ToString();
                                    var image = new BitmapImage(new Uri(imageFull, UriKind.Absolute));
                                    var encoder = new JpegBitmapEncoder();
                                    encoder.Frames.Add(BitmapFrame.Create(image));

                                    using (var stream = new MemoryStream())
                                    {
                                        encoder.Save(stream);
                                        oldProduct.Photos.First().Data = stream.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        // Nếu thêm
                        else
                        {
                            db.Products.Add(product);
                            db.SaveChanges();

                            //trường image
                            var imageFull = imgProduct.Tag.ToString();
                            var image = new BitmapImage(new Uri(imageFull, UriKind.Absolute));
                            var encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(image));

                            using (var stream = new MemoryStream())
                            {
                                encoder.Save(stream);
                                var photo = new Photo()
                                {
                                    ProductId = product.Product_Id,
                                    Data = stream.ToArray()
                                };
                                db.Photos.Add(photo);
                                db.SaveChanges();
                            }

                            // refresh lại textbox
                            Refresh();
                        }
                    }
                    db.SaveChanges();

                    // Nếu trang vừa rồi là danh sách sản phẩm thì cập nhật nó
                    if (RefreshProductList != null)
                    {
                        RefreshProductList.Invoke(true);
                    }

                    var dialog= new Messenge() { Message = "Added product Successful!" };
                    dialog.Owner = Window.GetWindow(this);
                    dialog.Sounds();
                    dialog.ShowDialog();

                }
                catch (Exception ex)
                {
                    //var dialogError1 = new Dialog() { Message = "Product id not exist!" };
                    //dialogError1.Owner = Window.GetWindow(this);
                    //dialogError1.ShowDialog();
                }
            }
        }
        public void Refresh()
        {
            productNameTextBox.Clear();
            storeCodeTextBox.Clear();
            productPriceTextbox.Clear();
            productQuantityTextbox.Clear();
            productDescriptionTextBox.Clear();
            ProductTypeComboxBox.SelectedIndex = -1;

            var img = new BitmapImage();

            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,/Images/Icon/defaultImage.png");
            img.EndInit();

            imgProduct.Source = img;
            imgProduct.Tag = null;
        }
        /// <summary>
        /// refesh lại comboBox category
        /// </summary>
        /// <param name="Data"></param>
        public void RefreshComboBox(bool Data)
        {
            if (Data) // Nếu có chỉnh sửa danh sách loại sản phẩm thì refresh comboBox
            {
                int oldIndex = ProductTypeComboxBox.SelectedIndex;

                // Get và hiển thị danh sách loại sản phẩm
                Thread getCategories = new Thread(delegate ()
                {
                    var db = new MyShopEntities();
                    var category = new ObservableCollection<Category>(db.Categories.ToList());
                    Dispatcher.Invoke(() => {

                        //gán lên comboBox category
                        ProductTypeComboxBox.ItemsSource = category;
                        if (oldIndex > 0) ProductTypeComboxBox.SelectedIndex = oldIndex;

                        // Cập nhật tiếp trang ở trước
                        if (RefreshProductList != null)
                        {
                            RefreshProductList.Invoke(true);
                        }
                    });
                });
                getCategories.Start();
            }
        }
    }
}
