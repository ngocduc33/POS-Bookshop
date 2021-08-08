using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using MyShop.Products;
using MyShop.Custom;

namespace MyShop.Orders
{
    /// <summary>
    /// Interaction logic for NewOrdersScreen.xaml
    /// </summary>
    public partial class NewOrdersScreen : UserControl
    {
        public Product product;

        /// <summary>
        /// View model cho danh sách sự kiện
        /// </summary>
        public class ProductViewModel : Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }

        // danh sách sản phẩm
        ObservableCollection<ProductViewModel> _listProduct = new ObservableCollection<ProductViewModel>();
        public NewOrdersScreen()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Lấy sản phẩm từ ProductDetail
        /// </summary>
        public NewOrdersScreen(Product _product)
        {
            InitializeComponent();
            product = _product;
            editProductId.Text = product.Product_Id.ToString();
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

        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            createOrdersScreen.Children.Clear();//Quay lại màn hình Dashboard
        }

        private void selectProductId_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var chooseProduct = new ProductScreen();
            chooseProduct.contentBack.Content = "Creating new orders";
            chooseProduct.PickProductId = (x) =>
            {
                product = x;
                editProductId.Text = product.Product_Name;

                // Cập nhật giá cả + giá bán + tổng (xóa rồi nhập lại để gọi hàm phát sinh)
                string numberBuy = numberBuyTextBox.Text;
                numberBuyTextBox.Text = "";
                numberBuyTextBox.Text = numberBuy;
            };
            createOrdersScreen.Children.Add(chooseProduct);
        }

        private void NumberBuy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (numberBuyTextBox.Text.Length > 0)
            {
                //Mở nút thêm
                btnAddToListProduct.Visibility = Visibility.Visible;

                // Kiểm tra còn đủ hàng hay không
                var db = new MyShopEntities();
                var curProduct = db.Products.Find(product.Product_Id);
                int number = 0;                                         // Số lượng mua
                int.TryParse(numberBuyTextBox.Text, out number);
                if (curProduct.Quantity < number)  // Nếu số lượng không đủ thì thông báo
                {
                    var dialogError = new Messenge() { Message = "Not enough quantity!" };
                    dialogError.Sounds();
                    dialogError.time = 2000;
                    dialogError.Owner = Window.GetWindow(this);
                    dialogError.ShowDialog();

                    numberBuyTextBox.Clear();

                    //Tắt nút thêm
                    btnAddToListProduct.Visibility = Visibility.Hidden;
                    return;
                }

                // Lấy số lượng người dùng muốn mua
                int NumberBuy = 0;
                int.TryParse(numberBuyTextBox.Text, out NumberBuy);

                // Phát sinh giá gốc
                double value = 0;
                value = (double)product.Price;
                originalPriceTextbox.Text = value.ToString("N0");

                //Sự kiện khuyến mãi (Chưa có)
                float eventPromotion = 0;
                sellPriceTextBox.Text = (value - value * eventPromotion / 100).ToString("N0");

                //tổng tiền
                totalTextBox.Text = (value * NumberBuy).ToString("N0");
            }
            else
            {
                btnAddToListProduct.Visibility = Visibility.Hidden;
                originalPriceTextbox.Clear();
                sellPriceTextBox.Clear();
                totalTextBox.Clear();
            }

            // Tính tiền trả lại + tiền sẽ thu nếu giao hàng
            CountExchange(null, null);
            CountMoneyWillGet(null, null);
        }

        /// <summary>
        /// Xử lý ô giá chỉ nhận giá trị số
        /// </summary>
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        ProductViewModel productViewModel = null;
        private void ListProducts_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btncancel.Visibility = Visibility.Visible;
            ProductViewModel viewModel = (sender as ListView).SelectedItem as ProductViewModel;
            productViewModel = viewModel;//Lấy giá trị item trong listView được mouse vào
        }
        /// <summary>
        /// Hủy sản phẩm được chọn
        /// </summary>
        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra có nhập sản phẩm hay không?
            if (productViewModel == null)
            {
                var dialogError = new Messenge() { Message = "Please select a product!" };
                dialogError.Sounds();
                dialogError.time = 2000;
                dialogError.Owner = Window.GetWindow(this);
                dialogError.ShowDialog();
                btncancel.Visibility = Visibility.Hidden;
                return;
            }
            else
            {
                //xóa 
                _listProduct.Remove(productViewModel);//Xóa đơn hàng được chọn trong listview

                //Cập nhật lại tổng sản phẩm
                double SumProducts = 0;
                foreach (var sumP in _listProduct)
                {
                    SumProducts += (double)sumP.Total;
                }

                //Cập nhật lại tổng tiền
                sumTotalOfProduct.Text = $"{SumProducts.ToString()} đ";
                btncancel.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Thêm sản phẩm vào danh sách sản phẩm
        /// </summary>
        private void btnAddToListProduct_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;

            double SumProducts = 0;
            //Không nhập số lượng
            if (numberBuyTextBox.Text == "")
            {
                var dialogError = new Messenge() { Message = "Please enter the quantity!" };
                dialogError.Sounds();
                dialogError.time = 2000;
                dialogError.Owner = Window.GetWindow(this);
                dialogError.ShowDialog();
                return;
            }

            //Tìm sản phẩm trong list xem có cùng với sản phẩm mới thêm vào không, nếu có thì gộp lại 
            foreach (var p in _listProduct.ToList())
            {
                if (editProductId.Text == p.ProductName)
                {
                    //Thêm sản phẩm mới, thay cho sản phẩm cũ
                    var newProductInListView = new ProductViewModel()
                    {
                        ProductID = product.Product_Id,
                        ProductName = product.Product_Name,
                        Price = decimal.Parse(product.Price.ToString()),
                        Quantity = int.Parse(numberBuyTextBox.Text) + p.Quantity,
                        Total = decimal.Parse(totalTextBox.Text) + p.Total,
                    };
                    _listProduct.Remove(p);
                    _listProduct.Add(newProductInListView);

                    //Tính tổng tiền
                    foreach (var sumP in _listProduct)
                    {
                        SumProducts += (double)sumP.Total;
                    }

                    listProducts.ItemsSource = _listProduct;
                    sumTotalOfProduct.Text = $"{SumProducts.ToString("N0")} đ";
                    numberBuyTextBox.Clear();
                    return;
                }
            }

            var productInListView = new ProductViewModel()
            {
                ProductID = product.Product_Id,
                ProductName = product.Product_Name,
                Price = decimal.Parse(product.Price.ToString()),
                Quantity = int.Parse(numberBuyTextBox.Text),
                Total = decimal.Parse(totalTextBox.Text),
            };

            _listProduct.Add(productInListView);

            foreach(var p in _listProduct)
            {
                SumProducts += (double)p.Total;
            }

            listProducts.ItemsSource = _listProduct;
            sumTotalOfProduct.Text = $"{SumProducts.ToString("N0")} đ";

            ProgressBar.IsIndeterminate = false;
            ProgressBar.Visibility = Visibility.Hidden;
            numberBuyTextBox.Clear();
            
        }
        /// <summary>
        /// Sự kiện khi chọn phương thức thanh toán (tắt các Control không liên quan)
        /// </summary>
        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button == rdoGoToShop)
            {
                if (editAddress != null) editAddress.Clear();
                if (editDeposit != null) editDeposit.Clear();
                if (editShipCost != null) editShipCost.Clear();
                if (editMoneyWillGet != null) editMoneyWillGet.Clear();
            }
            else
            {
                if (editMoneyTaken != null) editMoneyTaken.Clear();
                if (editMoneyExchange != null) editMoneyExchange.Clear();
            }
        }

        /// <summary>
        /// Tự động tính tiền trả lại (exchange)
        /// </summary>
        private void CountExchange(object sender, TextChangedEventArgs e)
        {
            if (editMoneyTaken.Text.Length > 0)
            {
                // Định dạng giá vừa nhập
                double taken = 0;
                double.TryParse(editMoneyTaken.Text, out taken);
                editMoneyTaken.Text = taken.ToString("N0");
                editMoneyTaken.CaretIndex = editMoneyTaken.Text.Length;

                // Tính toán số tiền thối lại
                double sellPrice = 0;
                double.TryParse(sellPriceTextBox.Text, out sellPrice);
                var sum = _listProduct.Sum((dynamic p) => (decimal)p.Total);
                editMoneyExchange.Text = (taken - (double)sum).ToString("N0");
            }
            else editMoneyExchange.Clear();
        }

        /// <summary>
        /// Tự động tính tiền sẽ thu khi đi giao hàng
        /// </summary>
        private void CountMoneyWillGet(object sender, TextChangedEventArgs e)
        {
            if (editDeposit.Text.Length > 0 || editShipCost.Text.Length > 0)
            {
                double Sum = 0; // Tổng tiền
                //
                var Total = _listProduct.Sum((dynamic p) => (decimal)p.Total);
                double.TryParse(Total.ToString(), out Sum);

                double onlinePay = 0; // Tiền thanh toán online
                double.TryParse(editDeposit.Text, out onlinePay);
                editDeposit.CaretIndex = editDeposit.Text.Length;
                editDeposit.Text = onlinePay.ToString("N0");

                double shipCost = 0; // Phí ship
                double.TryParse(editShipCost.Text, out shipCost);
                editShipCost.CaretIndex = editShipCost.Text.Length;
                editShipCost.Text = shipCost.ToString("N0");

                // Tính số tiền cần thu khi đi giao
                editMoneyWillGet.Text = (Sum - onlinePay + shipCost).ToString("N0");
            }
            else
            {
                editDeposit.Clear();
                editShipCost.Clear();
                editMoneyWillGet.Clear();
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu có thiếu không
            if ((rdoGoToShop.IsChecked == true && editMoneyTaken.Text.Length == 0)   // Nếu thanh toán trực tiếp mà chưa đưa tiền
                || (rdoShip.IsChecked == true && editAddress.Text.Length == 0)          // Nếu thanh toán giao hàng mà không đưa địa chỉ
                || (rdoShip.IsChecked == true && editMoneyWillGet.Text.Length == 0))    // Nếu thanh toán giao hàng mà không biết số tiền sẽ thu
            {
                var dialogError = new Dialog() { Message = "Please enter the full information!" };
                dialogError.Sounds(); 
                dialogError.Owner = Window.GetWindow(this);
                dialogError.ShowDialog();
                return;
            }

            string customer_tel = customerPhoneTextBox.Text.Length == 0 ? null : customerPhoneTextBox.Text;
            var db = new MyShopEntities();

            //Khách hàng có trong cơ sở dữ liệu => thì nhập đúng số điện thoại thì tự động hiển thị tên
            //Khách hàng mới (có điền thông tin) => khách hàng mới => tạo khách hàng mới
            //Khách hàng vãng lai (có bộ chuyển đổi Converter để chuyển customerPhoneTextBox khi rỗng)

            //Xác nhận thêm hóa đơn
            var dialogNotification = new Dialog() { Message = "Confirm want to add?" };
            dialogNotification.Sounds();
            dialogNotification.Owner = Window.GetWindow(this);
            dialogNotification.ShowDialog();
            if (true == dialogNotification.DialogResult)
            {
                //Thao tác để thêm vào CSDL
                try
                {
                    //Thêm mới khách hàng nếu không tìm thấy khách hàng trong cơ sở dữ liệu (khách hàng mới)
                    if (customer == null && customerPhoneTextBox.Text.Length != 0)
                    {
                        //Tạo thông tin khách hàng
                        var newCustomer = new Customer()
                        {
                            Tel = customerPhoneTextBox.Text,
                            Fullname = CustomerNameTextBox.Text.Length < 0 ? null : CustomerNameTextBox.Text,
                            Address = null,
                        };
                        db.Customers.Add(newCustomer);
                        db.SaveChanges();//lưu
                    }

                    // - Kiểm tra xem khách hàng thanh toán bằng hình thức nào
                    //    + Nếu thanh toán trực tiếp và thanh toán đủ tiền => hoàn thành đơn hàng
                    //    + Còn ngược lại thì sẽ lưu địa chỉ nơi nhận hàng và tiền ship

                    //Thêm hóa đơn mới
                    var bill = new Purchase()
                    {
                        CustomerTel = customer_tel,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Total = _listProduct.Sum((dynamic p) => (decimal)p.Total),
                        AtStore = (bool)rdoGoToShop.IsChecked,
                        MoneyTaken = (bool)rdoGoToShop.IsChecked ? decimal.Parse(editMoneyTaken.Text) : 0,
                        MoneyExchange = (bool)rdoGoToShop.IsChecked ? decimal.Parse(editMoneyExchange.Text) : 0,
                        DeliveryAdress = (bool)rdoShip.IsChecked ? editAddress.Text : null,
                        Deposit = (bool)rdoShip.IsChecked ? decimal.Parse(editDeposit.Text) : 0,
                        Ship = (bool)rdoShip.IsChecked ? decimal.Parse(editShipCost.Text) : 0,
                        MoneyWillGet = (bool)rdoShip.IsChecked ? decimal.Parse(editMoneyWillGet.Text) : 0,
                        Status = (bool)rdoGoToShop.IsChecked ? 2 : 1

                    };
                    db.Purchases.Add(bill);
                    db.SaveChanges();

                    //Thêm danh sách sản phẩm được order
                    for (int i = 0; i < _listProduct.Count; i++)
                    {
                        //Tạo order với chi tiết đơn đặt hàng
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = bill.Purchase_Id,
                            ProductId = _listProduct[i].ProductID,
                            Price = _listProduct[i].Price,
                            Quantity = _listProduct[i].Quantity,
                            Total = _listProduct[i].Total,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();//lưu

                        //nếu tạo đơn hàng với sản phâm đó thì mặc nhiên là sẽ trừ số lượng của sản phẩm đó đi
                        var updateQuantity = db.Products.Find(_listProduct[i].ProductID);
                        if (updateQuantity != null)
                        {
                            //lấy số lượng hiện tại trong CSDL và trừ đi số lượng sản phẩm đã thanh toán
                            updateQuantity.Quantity -= _listProduct[i].Quantity;
                            db.SaveChanges();
                        }
                        //Nếu mà khách trả lại hàng thì cập nhật lại số lượng lại cho product, sẽ tính sau
                    }
                    db.SaveChanges();

                    //Thông báo khi thêm order thành công
                    var dialogSuccessfully = new Messenge() { Message = "Added an order successfully" };
                    dialogSuccessfully.Sounds();
                    dialogSuccessfully.Owner = Window.GetWindow(this);
                    dialogSuccessfully.time = 2000;
                    dialogSuccessfully.ShowDialog();
                }
                catch(Exception ex)
                {
                    
                }
            }

            //Làm mới 
            editProductId.Clear();
            for (int i = 0; i < _listProduct.Count; i++)
            {
                _listProduct.RemoveAt(i);
            }
            sumTotalOfProduct.Text = "0 đ";
            rdoGoToShop.IsChecked = true;

        }
        /// <summary>
        /// Show màn hình orders
        /// </summary>
       

        Customer customer = null;
        /// <summary>
        /// Tìm xem khách hàng này có trong CSDL không
        /// </summary>
        private void customerPhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var telCustomer = customerPhoneTextBox.Text;
            var db = new MyShopEntities();
            customer = db.Customers.Find(telCustomer);//Tìm số điện thoại trong CSDL

            //Tìm thấy tên khách hàng theo số điện thoại
            if (customer != null)
            {
                CustomerNameTextBox.Text = customer.Fullname;//Lấy tên tương ướng với SDT
            }
            else
            {
                CustomerNameTextBox.Clear();
            }
        }

    }
}
