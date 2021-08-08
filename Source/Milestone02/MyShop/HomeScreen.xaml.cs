using System;
using System.Collections.Generic;
using System.IO;
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
using Aspose.Cells;
using Microsoft.Win32;
using MyShop.Products;
using MyShop.Categories;
using MyShop.Custom;
using MyShop.Orders;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : UserControl
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void ProductList_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            dashboard.Children.Clear();
            dashboard.Children.Add(new ProductScreen());
        }

        /// <summary>
        /// Import data từ file excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportExcel_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            var imPortExel = new OpenFileDialog();
            if (imPortExel.ShowDialog() == true)
            {
                var filename = imPortExel.FileName;
                var fileinfo = new FileInfo(filename);

                var excelFile = new Workbook(filename);
                var tabs = excelFile.Worksheets;

                var db = new MyShopEntities();
                foreach (var tab in tabs)
                {
                    var category = new Category()
                    {
                        Category_Name = tab.Name
                    };
                    db.Categories.Add(category);
                    db.SaveChanges();

                    var row = 3;

                    var cell = tab.Cells[$"C3"];
                    while (cell.Value != null)
                    {
                        var product = new Product()
                        {
                            CatId = category.Category_Id,
                            SKU = tab.Cells[$"C{row}"].StringValue,
                            Product_Name = tab.Cells[$"D{row}"].StringValue,
                            Price = tab.Cells[$"E{row}"].IntValue,
                            Quantity = tab.Cells[$"F{row}"].IntValue,
                            Description = tab.Cells[$"G{row}"].StringValue
                        };

                        category.Products.Add(product);
                        db.SaveChanges();

                        var imageName = tab.Cells[$"H{row}"].StringValue;
                        var imageFull = $"{fileinfo.Directory}\\images\\{imageName}";
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
                        row++;
                        cell = tab.Cells[$"C{row}"];
                    }

                }

                var dialog = new Dialog() { Message = "Successfully imported data!" };
                dialog.SoundsSuccessful();
                dialog.Owner = Window.GetWindow(this);
                dialog.ShowDialog();

            }

        }

        private void listCategories_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            dashboard.Children.Add(new CategoryScreen());
        }

        private void createBill_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            dashboard.Children.Add(new OrdersScreen());
        }

        private void ReportProduct_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            dashboard.Children.Add(new ReportScreen());
        }
    }
}
