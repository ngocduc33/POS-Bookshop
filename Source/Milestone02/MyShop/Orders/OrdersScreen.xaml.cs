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

namespace MyShop.Orders
{
    /// <summary>
    /// Interaction logic for OrdersScreen.xaml
    /// </summary>
    public partial class OrdersScreen : UserControl
    {
        MyShopEntities db = new MyShopEntities();
        PagingInfo _pagingInfo;
        int rowPerPage = 5;
        public OrdersScreen()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Quay lại trang tạo order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            screenOrders.Children.Clear();
        }

        void CalculatePagingInfo()
        {
            int status = (statusFilterComboBox.SelectedItem as OrderState).OrderState_Key;
            var keyword = string.IsNullOrEmpty(searchTextBox.Text) ? " " : searchTextBox.Text;

            IQueryable<Purchase> dateFilter;
            if (fromDayDatePicker.SelectedDate != null && toDayDatePicker.SelectedDate != null)
            {
                var from = (DateTime)fromDayDatePicker.SelectedDate;
                var to = (DateTime)toDayDatePicker.DisplayDate;
                dateFilter = db.Purchases.Where(p => (from <= p.CreatedAt) && (p.CreatedAt <= to));
            }
            else
            {
                dateFilter = db.Purchases;
            }

            var query = dateFilter.
                GroupJoin(
                    db.Customers,
                    p => p.CustomerTel,
                    c => c.Tel,
                    (x, y) => new
                    {
                        Purchase = x,
                        Customer = y
                    }).
                SelectMany(
                    x => x.Customer.DefaultIfEmpty(),
                    (x, y) => new
                    {
                        Purchase = x.Purchase,
                        Customer = y
                    }).
                 Select(item => new
                 {
                     item.Purchase.Status,
                     item.Purchase.Purchase_Id,
                     item.Purchase.Total,
                     item.Purchase.CreatedAt,
                     item.Customer.Fullname,
                     item.Customer.Tel,
                     Details = item.Purchase.
                                      OrderDetails.Join(
                                                     db.Products,
                                                     d => d.ProductId,
                                                     p => p.Product_Id,
                                                     (d, p) => new
                                                     {
                                                         p.Product_Name,
                                                         d.Quantity,
                                                         d.Price,
                                                         d.Total
                                                     })
                 }).
                  OrderByDescending(p =>
                     p.CreatedAt)
                  .Join(db.OrderStates,
                      item => item.Status,
                      s => s.OrderState_Key,
                      (item, s) => new {
                          item.Status,
                          item.Purchase_Id,
                          item.Total,
                          item.CreatedAt,
                          item.Fullname,
                          item.Tel,
                          s.Display_Text,
                          item.Details,
                      });

            if (status != -1)
            {
                var subquery = from order in query
                               where order.Status == status &&
                                     (order.Tel.ToLower().Contains(keyword.ToLower()) 
                                     || order.Fullname.ToLower().Contains(keyword.ToLower()))
                               select new
                               {
                                   order.Status,
                                   order.Purchase_Id,
                                   order.Total,
                                   order.CreatedAt,
                                   order.Fullname,
                                   order.Tel,
                                   order.Display_Text,
                                   order.Details
                               };
                int count = subquery.Count();
                //var subquery = query.Where(p => p.Status == status).Count();//số lượng
                _pagingInfo = new PagingInfo()
                {
                    RowsPerPage = rowPerPage,
                    TotalItems = count,
                    TotalPages = count / rowPerPage + (((count % rowPerPage) == 0) ? 0 : 1),
                    CurrentPage = 1
                };

                pagingComboBox.ItemsSource = _pagingInfo.Pages;
                pagingComboBox.SelectedIndex = 0;
            }
            else
            {
                var subquery = from order in query
                               where order.Tel.ToLower().Contains(keyword.ToLower()) 
                                     || order.Fullname.ToLower().Contains(keyword.ToLower() ?? string.Empty)
                               select new
                               {
                                   order.Status,
                                   order.Purchase_Id,
                                   order.Total,
                                   order.CreatedAt,
                                   order.Fullname,
                                   order.Tel,
                                   order.Display_Text,
                                   order.Details
                               };


                int count = subquery.Count();//số lượng
                _pagingInfo = new PagingInfo()
                {
                    RowsPerPage = rowPerPage,
                    TotalItems = count,
                    TotalPages = count / rowPerPage + (((count % rowPerPage) == 0) ? 0 : 1),
                    CurrentPage = 1
                };

                pagingComboBox.ItemsSource = _pagingInfo.Pages;
                pagingComboBox.SelectedIndex = 0;
            }
        }

        void LoaddAllPurchase()
        {
            int status = (statusFilterComboBox.SelectedItem as OrderState).OrderState_Key;
            var keyword = string.IsNullOrEmpty(searchTextBox.Text) ? " " : searchTextBox.Text;

            IQueryable<Purchase> dateFilter;
            if (fromDayDatePicker.SelectedDate != null && toDayDatePicker.SelectedDate != null)
            {
                var from = (DateTime)fromDayDatePicker.SelectedDate;
                var to = (DateTime)toDayDatePicker.DisplayDate;
                dateFilter = db.Purchases.Where(p => (from <= p.CreatedAt) && (p.CreatedAt <= to));
            }
            else
            {
                dateFilter = db.Purchases;
            }

            var query = dateFilter.
                GroupJoin(
                    db.Customers,
                    p => p.CustomerTel,
                    c => c.Tel,
                    (x, y) => new
                    {
                        Purchase = x,
                        Customer = y
                    }).
                SelectMany(
                    x => x.Customer.DefaultIfEmpty(),
                    (x, y) => new
                    {
                        Purchase = x.Purchase,
                        Customer = y
                    }).
                 Select(item => new
                 {
                     item.Purchase.Status,
                     item.Purchase.Purchase_Id,
                     item.Purchase.Total,
                     item.Purchase.CreatedAt,
                     item.Customer.Fullname,
                     item.Customer.Tel,
                     item.Purchase.DeliveryAdress,
                     item.Purchase.AtStore,
                     Details = item.Purchase.OrderDetails.Join(
                                                            db.Products,
                                                            d=>d.ProductId,
                                                            p=>p.Product_Id,
                                                            (d, p) => new
                                                            {
                                                                p.Product_Name,
                                                                d.Quantity,
                                                                d.Price,
                                                                d.Total,
                                                                
                                                            })
                 }).
                  OrderByDescending(p =>
                     p.CreatedAt)
                  .Join(db.OrderStates,
                      item => item.Status,
                      s => s.OrderState_Key,
                      (item, s) => new{
                          item.Status,
                          item.Purchase_Id,
                          item.Total,
                          item.CreatedAt,
                          item.Fullname,
                          item.Tel,
                          item.DeliveryAdress,
                          item.AtStore,
                          s.Display_Text,
                          item.Details,
                      });

            if (status != -1)
            {
                var subquery = from order in query
                               where order.Status == status &&
                                     ( order.Tel.ToLower().Contains(keyword.ToLower())
                                     || order.Fullname.ToLower().Contains(keyword.ToLower() ?? string.Empty))
                               select new
                               {
                                   order.Status,
                                   order.Purchase_Id,
                                   order.Total,
                                   order.CreatedAt,
                                   order.Fullname,
                                   order.Tel,
                                   order.DeliveryAdress,
                                   order.AtStore,
                                   order.Display_Text,
                                   order.Details
                               };

                //var subquery = query.Where(p => p.Status == status).OrderByDescending(x => x.CreatedAt);
                var skip = ((_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage) < 0 ? 0: (_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage;
                var take = _pagingInfo.RowsPerPage;
                listOrders.ItemsSource = subquery.OrderByDescending(x => x.CreatedAt).Skip(skip).Take(take).ToList();
            }
            else
            {
                var subquery = from order in query
                               where order.Tel.ToLower().Contains(keyword.ToLower())
                                     || order.Fullname.ToLower().Contains(keyword.ToLower() ?? string.Empty)
                               select new
                               {
                                   order.Status,
                                   order.Purchase_Id,
                                   order.Total,
                                   order.CreatedAt,
                                   order.Fullname,
                                   order.Tel,
                                   order.DeliveryAdress,
                                   order.AtStore,
                                   order.Display_Text,
                                   order.Details
                               };
                //var subquery = query.OrderByDescending(x=>x.CreatedAt);
                var skip = ((_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage) < 0 ? 0 : (_pagingInfo.CurrentPage - 1) * _pagingInfo.RowsPerPage;
                var take = _pagingInfo.RowsPerPage;
                listOrders.ItemsSource = subquery.OrderByDescending(x => x.CreatedAt).Skip(skip).Take(take).ToList();
            }
        }

        /// <summary>
        /// Load danh sách orders
        /// </summary>
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            var AllOrderState = db.OrderStates.ToList();
            statusFilterComboBox.ItemsSource = AllOrderState;
            statusFilterComboBox.SelectedIndex = 0;
            //LoaddAllPurchase();
        }
        /// <summary>
        /// Trạng thái của đơn hàng
        /// </summary>
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePagingInfo();
            LoaddAllPurchase();
        }
        private void searchTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CalculatePagingInfo();
            LoaddAllPurchase();
        }

        #region Hiệu ứng ComboBox
        /// <summary>
        /// Xử lý hiệu ứng khi chọn vào combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboFilter_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.LightGray;
        }
        /// <summary>
        /// Hiệu ứng khi bỏ chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboFilter_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.Transparent;
        }
        #endregion

        /// <summary>
        /// Trang trước đó nếu như trang trước đó > 0
        /// </summary>
        private void PreviousPaging_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = pagingComboBox.SelectedIndex;
            if (currentIndex > 0)
            {
                pagingComboBox.SelectedIndex--;
            }
        }
        /// <summary>
        /// Trang sau đó nếu như trang sau đó <= trang cuối cùng
        /// </summary>
        private void NextPaging_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = pagingComboBox.SelectedIndex;
            if (currentIndex <= _pagingInfo.Pages.Count)
            {
                pagingComboBox.SelectedIndex++;
            }
        }
        /// <summary>
        /// paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageIndexComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int nextPage = pagingComboBox.SelectedIndex + 1;
            _pagingInfo.CurrentPage = nextPage;
            LoaddAllPurchase();
        }
        /// <summary>
        /// Chỉnh sửa orders
        /// </summary>
        private void editOrderMouseUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dynamic item = listOrders.SelectedItem;

            var screen = new EditOrdersScreen(item.Purchase_Id);
            screen.RefreshOrdersList = Refresh;
            screenOrders.Children.Add(screen);

        }
        /// <summary>
        /// xóa một orders
        /// </summary>
        private void deleteOrderMouseUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //hiển thị thông báo muốn xóa hay không? 
            var dialog = new Dialog() { Message = "Delete the selected order?" };
            dialog.Sounds();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == false) return;

            //lấy item đã chọn trong listview
            dynamic itemOrderListView = listOrders.SelectedItem;
            try
            {
                //tìm order theo id
                var order = db.Purchases.Find(itemOrderListView.Purchase_Id);
                db.Purchases.Remove(order);
                db.SaveChanges();//lưu lại thay đổi

                //cập nhật lại danh sách
                CalculatePagingInfo();
                LoaddAllPurchase();

                //hiển thị thông báo thành công
                var dialogNotification = new Messenge() { Message = "Order deleted successfully!" };
                dialogNotification.Sounds();
                dialogNotification.Owner = Window.GetWindow(this);
                dialogNotification.ShowDialog();

            }
            catch(Exception ex)
            {

            }


            
        }

        private void fromDayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void toDayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fromDayDatePicker.SelectedDate != null)
            {
                var from = (DateTime)fromDayDatePicker.SelectedDate;
                var to = (DateTime)toDayDatePicker.SelectedDate;

                if (DateTime.Compare(from, to) < 0)
                {
                    CalculatePagingInfo();
                    LoaddAllPurchase();
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
        /// <summary>
        /// Làm mới lại danh sách khi edit một order bên màn hình edit order
        /// </summary>
        public void Refresh(bool Data)
        {
            if (!Data) return;

            CalculatePagingInfo();
            LoaddAllPurchase();
        }


        private void Create_Order_Click(object sender, RoutedEventArgs e)
        {
            screenOrders.Children.Add(new NewOrdersScreen());
        }
    }
}
