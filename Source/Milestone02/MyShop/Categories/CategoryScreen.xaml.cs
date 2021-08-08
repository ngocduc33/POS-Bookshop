using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using MyShop.Custom;

namespace MyShop.Categories
{
    /// <summary>
    /// Interaction logic for CategoryScreen.xaml
    /// </summary>
    public partial class CategoryScreen : UserControl
    {        
        // Delegate để refresh combobox ở NewProductPage
        public delegate void DelegateRefeshCategoriesList(bool Data);
        public DelegateRefeshCategoriesList RefreshCategoriesList;

        ObservableCollection<Category> _categories;

        public CategoryScreen()
        {
            InitializeComponent();

            // Get và hiển thị danh sách loại sản phẩm
            Thread getCategories = new Thread(delegate ()
            {
                // Get và hiển thị danh sách loại sản phẩm
                var db = new MyShopEntities();

                // ObservableCollection có implements INotifyCollectionChanged interface
                _categories = new ObservableCollection<Category>(db.Categories);

                // Đặt Item Source cho List View
                Dispatcher.Invoke(() => {
                    listCategories.ItemsSource = _categories;
                    progressBarLoadCategories.IsEnabled = false;
                    progressBarLoadCategories.Visibility = Visibility.Hidden;
                });
            });
            getCategories.Start();
        }

        //xóa màn hình 
        private void backWard_Click(object sender, RoutedEventArgs e)
        {
            categoriesScreen.Children.Clear();
        }
        /// <summary>
        /// bắt sự kiện khi bắt click trên item listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // item của listview không tồn tại
            if (listCategories.SelectedItem == null) return;
            var category = listCategories.SelectedItem as Category;

            //gán text cho các textbox
            categoryIdTextBox.Text = category.Category_Id.ToString();
            categoryNameTextBox.Text = category.Category_Name;
            categoryDescriptionTextBox.Text = category.Description;

            //mở nút update và delete
            btnUpdateCategory.IsEnabled = true;
            btnDeleteCategory.IsEnabled = true;
        }
        /// <summary>
        /// Thêm một loại sản phẩm, sau đó đưa vào cơ sở dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            // tắt chọn listview
            if (listCategories.Items.Count > 0)
            {
                listCategories.ScrollIntoView(listCategories.Items[0]);
            }
            listCategories.SelectedIndex = -1;
            listCategories.IsEnabled = false;


            // Bật và thay nội dung 2 button Sửa và Xóa
            btnUpdateCategory.IsEnabled = true;
            btnUpdateCategory.Content = "Save";
            btnUpdateCategory.Tag = "Add";

            btnDeleteCategory.IsEnabled = true;
            btnDeleteCategory.Content = "Cancel";

            // Tắt button Thêm
            btnAddCategory.IsEnabled = false;

            //id tự động
            categoryIdTextBox.Clear();
            categoryIdTextBox.Text = "Auto";
            categoryIdTextBox.IsEnabled = false;

            // Cho phép người dùng chỉnh sửa các TextBox
            categoryNameTextBox.IsEnabled = true;
            categoryNameTextBox.Clear();

            categoryDescriptionTextBox.IsEnabled = true;
            categoryDescriptionTextBox.Clear();

            // Focus vào Tên loại sản phẩm cho người dùng nhập
           categoryNameTextBox.Focus();

        }
        /// <summary>
        /// Thêm và Sửa category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            // content là sửa
            if (btnUpdateCategory.Content.Equals("Edit"))
            {
                // Thay nội dung các button
                btnUpdateCategory.Content = "Save";// đổi content là lưu
                btnUpdateCategory.Tag = "Edit"; // lưu đường dẫn btnUpdateCategory là sửa

                btnDeleteCategory.Content = "Cancel";
                btnAddCategory.IsEnabled = false;// tắt nút add

                // Cho phép chỉnh sửa các TextBox, chỉ cho phép sửa tên là mô tả
                categoryIdTextBox.IsEnabled = false;
                categoryNameTextBox.IsEnabled = true;
                categoryDescriptionTextBox.IsEnabled = true;

                categoryNameTextBox.Focus();

                // Tạm thời vô hiệu hóa List View
                listCategories.IsEnabled = false;
            }

            // Content của button btnUpdateCategory là "lưu"
            else
            {
                // Kiểm tra dữ liệu có nhập đầy đủ không
                if (categoryIdTextBox.Text.Length == 0
                    || categoryNameTextBox.Text.Length == 0)
                {
                    var dialogError1 = new Dialog() { Message = "Please enter all the information!" };
                    dialogError1.Owner = Window.GetWindow(this);
                    dialogError1.ShowDialog();
                    return;
                }

                // Tạo đối tượng từ các TextBox
                var category = new Category()
                {
                    //ID sẽ tự động tăng lên ở SQL
                    Category_Name = categoryNameTextBox.Text.Length == 0 ? null : categoryNameTextBox.Text,
                    //Id = editProductTypeId.Text.Length == 0 ? null : editProductTypeId.Text,
                    Description = categoryDescriptionTextBox.Text.Length == 0 ? null : categoryDescriptionTextBox.Text
                };

                #region THAO TÁC VỚI CSDL
                // Nếu xác nhận thêm mới (1 nút 2 chức năng, lưu khi thêm và lưu khi sửa)
                if (btnUpdateCategory.Tag.Equals("Add"))
                {
                    // Hiện thông báo xác nhận
                    var dialog = new Dialog() { Message = "Add category, Are you sure?" };
                    dialog.Owner = Window.GetWindow(this);
                    if (dialog.ShowDialog() == false) return;

                    var db = new MyShopEntities();
                    try
                    {
                        db.Categories.Add(category);
                        db.SaveChanges();

                        // Cập nhật lên List View
                        _categories.Add(category);
                        listCategories.SelectedIndex = _categories.Count - 1;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                // Nếu xác nhận sửa
                if (btnUpdateCategory.Tag.Equals("Edit"))
                {
                    // Hiện thông báo xác nhận
                    var dialog = new Dialog() { Message = "Edit category, Are you sure?" };
                    dialog.Owner = Window.GetWindow(this);
                    if (dialog.ShowDialog() == false) return;

                    var db = new MyShopEntities();

                    // Kiểm tra xem user có sửa mã loại hay không (so sánh với List View)
                    if (category.Category_Id == ((Category)listCategories.SelectedItem).Category_Id)
                    {
                        // Tìm ID tương đương với category
                        var target = db.Categories.Find(category.Category_Id);
                        if (target != null)
                        {
                            // Sửa name và description
                            target.Category_Name = category.Category_Name;
                            target.Description = category.Description;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        // Nếu sửa ID thì sửa tất cả product thuộc category đó
                        var dialogNotification = new Dialog() 
                        { 
                            Message = "Edit id category,You need to fix all corresponding products?"
                        };
                        dialogNotification.Sounds();
                        dialogNotification.Owner = Window.GetWindow(this);

                        // Nếu đồng ý, thực hiện sửa lại tất cả sản phẩm tương ứng
                        if (dialogNotification.ShowDialog() == true)
                        {
                            //Kiểm tra mã mới đã tồn tại chưa
                            if (db.Categories.Find(category.Category_Id) != null)
                            {
                                var dialogError = new Dialog() {
                                    Message = "ID category exist!"
                                };
                                dialogError.Owner = Window.GetWindow(this);
                                dialogError.ShowDialog();
                                return;
                            }

                            //Thêm đối tượng dữ liệu mới (mã loại mới)
                            db.Categories.Add(category);

                            //Tìm sản phẩm tương đương với category => sửa tất cả
                            var list = db.Products.Where(x => x.CatId == ((Category)listCategories.SelectedItem).Category_Id).ToList();
                            for (int i = 0; i < list.Count; i++) list[i].CatId = category.Category_Id;

                            //Xóa dữ liệu cũ
                            var target = db.Categories.Find(((Category)listCategories.SelectedItem).Category_Id);
                            if (target != null) db.Categories.Remove(target);
                            db.SaveChanges();
                        }
                    }

                    // Cập nhật lên List View
                    int curIndex = listCategories.SelectedIndex;
                    _categories.Insert(curIndex + 1, category);
                    _categories.RemoveAt(curIndex);
                    listCategories.SelectedIndex = curIndex;
                }
                #endregion

                // Reset nội dung 2 button Sửa và Xóa
                btnUpdateCategory.Content = "Edit";
                btnDeleteCategory.Content = "Delete";

                // Vô hiệu hóa các TextBox
                categoryIdTextBox.IsEnabled = false;
                categoryNameTextBox.IsEnabled = false;
                categoryDescriptionTextBox.IsEnabled = false;

                // Bật lại button Thêm và List View
                btnAddCategory.IsEnabled = true;
                listCategories.IsEnabled = true;

                // Cập nhật combobox ở trang trước
                if (RefreshCategoriesList != null)
                {
                    RefreshCategoriesList.Invoke(true);
                }
            }
        }
        /// <summary>
        /// Xóa một loại sản phẩm, nếu loại sản phẩm không có sản phẩm => Xóa
        /// nếu có sẽ hỏi người dùng xóa hay không
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeleteCategory.Content.Equals("Cancel"))
            {
                // không cho phép chỉnh sửa các TextBox
                categoryIdTextBox.IsEnabled = false;
                categoryNameTextBox.IsEnabled = false;
                categoryDescriptionTextBox.IsEnabled = false;

                // Làm sạch các TextBox nếu vừa định Thêm mới
                if (btnUpdateCategory.Tag.Equals("Add"))
                {
                    categoryIdTextBox.Clear();
                    categoryNameTextBox.Clear();
                    categoryDescriptionTextBox.Clear();

                    // Tắt luôn hai button Sửa và Xóa (nếu vừa định Sửa thì thôi)
                    btnUpdateCategory.IsEnabled = false;
                    btnDeleteCategory.IsEnabled = false;
                }
                // Reset dữ liệu cũ nếu vừa định Sửa
                else
                {
                    // Lấy đối tượng ProductType tương ứng
                    var  productType = listCategories.SelectedItem as Category;

                    categoryNameTextBox.Text = productType.Category_Name;
                    //editProductTypeId.Text = productType.Id;
                    categoryDescriptionTextBox.Text = productType.Description;
                }

                // Reset nội dung 2 button Sửa và Xóa
                btnUpdateCategory.Content = "Edit";
                btnDeleteCategory.Content = "Delete";
                btnAddCategory.IsEnabled = true;

                // Bật lại List View
                listCategories.IsEnabled = true;
            }
            else
            {
                // Hiện thông báo xác nhận
                var dialog = new Dialog() { Message = "Delete the selected category?" };
                dialog.Owner = Window.GetWindow(this);
                if (dialog.ShowDialog() == false) return;

                // Lấy đối tượng từ List View
                var selectedItem = listCategories.SelectedItem as Category;

                // Tìm đối tượng tương ứng trong CSDL và xóa
                var db = new MyShopEntities();
                try
                {
                    var type = db.Categories.Where(x => x.Category_Id == selectedItem.Category_Id).FirstOrDefault();
                    db.Categories.Remove(type);
                    db.SaveChanges();


                    // Cập nhật
                    _categories.RemoveAt(listCategories.SelectedIndex);
                    listCategories.ItemsSource = null;
                    listCategories.ItemsSource = _categories;
                }
                catch (Exception ex)
                {
                    // Nếu bắt ngoại lệ (khóa ngoại) tức đang có sản phẩm thuộc loại muốn xóa
                    var showDialogError = new Dialog()
                    {
                        Message = "Product existence in Categories" +
                                    "\nDelete all?"
                    };
                    showDialogError.Sounds();
                    showDialogError.Owner = Window.GetWindow(this);

                    // Nếu user đồng ý, thì xóa hết sản phẩm tương ứng
                    if (showDialogError.ShowDialog() == true)
                    {
                        //Trước tiên phải xóa image của sản phẩm ở bảng Photo
                        db.Photos.RemoveRange(db.Photos.Where(x => x.Product.CatId == selectedItem.Category_Id).ToList());

                        // Tìm danh sách sản phẩm tương ứng sao đó RemoveRange
                        db.Products.RemoveRange(db.Products.Where(x => x.CatId == selectedItem.Category_Id).ToList());

                        // Sau cùng mới xóa loại sản phẩm
                        db.Categories.Remove(db.Categories.Where(x => x.Category_Id == selectedItem.Category_Id).FirstOrDefault());

                        // Cập nhật lên List View
                        _categories.Remove(selectedItem);
                        listCategories.ItemsSource = null;
                        listCategories.ItemsSource = _categories;
                        db.SaveChanges();
                    }
                    else return;
                }

                // Xóa xong thì tắt 2 nút Sửa và Xóa + làm sạch TextBox
                btnUpdateCategory.IsEnabled = false;
                btnDeleteCategory.IsEnabled = false;

                categoryIdTextBox.Clear();
                categoryNameTextBox.Clear();
                categoryDescriptionTextBox.Clear();

                // Cập nhật combobox ở trang trước, nếu là chọn category
                if (RefreshCategoriesList != null)
                {
                    RefreshCategoriesList.Invoke(true);
                }
            }
        }
    }
}
