using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyShop.Custom;
using MyShop.Login;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var Version = Assembly.GetExecutingAssembly().GetName().Version;

            version.Content = $"v{Version}";
        }

        public class DataProvider
        {
            private static DataProvider _ins;
            public static DataProvider Ins
            {
                get
                {
                    if (_ins == null) _ins = new DataProvider(); return _ins;
                }
                set
                {
                    _ins = value;
                }
            }
            public MyShopEntities db { get; set; }
            private DataProvider()
            {
                db = new MyShopEntities();

            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnLogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public void btnCommands_Click(object sender, RoutedEventArgs e)
        {
            Button curButton = sender as Button;
            if (curButton.Tag.Equals("btnClose"))
            {
                this.Close();
            }
            else if (curButton.Tag.Equals("btnMinim"))
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (curButton.Tag.Equals("btnMaxim"))
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
        }
        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string _username = "";
            string _password = "";
            _username = tbUser.Text;
            _password = MD5Hash(Base64Encode(tbPass.Password));

            var accCount = DataProvider.Ins.db.Users.Where(x => x.UserName.ToString() == _username && x.Password.ToString() == _password).ToList().Count();
            // Kiểm tra nếu chưa nhập username hay password
            if (tbUser.Text.Length == 0 && tbPass.Password.Length == 0)
            {
                // Hiện thông báo lỗi
                var dialog = new Messenge() { Message = "Please enter your account and password" };
                dialog.Sounds();
                dialog.time = 2000;
                dialog.Owner = Window.GetWindow(this);
                dialog.ShowDialog();
                return;
            }
            else if (tbUser.Text.Length == 0)
            {
                // Hiện thông báo lỗi
                var dialog = new Messenge() { Message = "Please enter your account" };
                dialog.Sounds();
                dialog.time = 2000;
                dialog.Owner = Window.GetWindow(this);
                dialog.ShowDialog();
                return;
            }
            else if (tbPass.Password.Length == 0)
            {
                // Hiện thông báo lỗi
                var dialog = new Messenge() { Message = "Please enter a password" };
                dialog.Sounds();
                dialog.time = 2000;
                dialog.Owner = Window.GetWindow(this);
                dialog.ShowDialog();
                return;
            }

            
            //Nếu đăng nhập thành công, ẩn màn hình login
            if (accCount > 0)
            {
                
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Close();
            }
            // Nếu thông tin tài khoản nhập vào sai
            else
            {
                // Hiện thông báo lỗi
                var dialog = new Messenge() { Message = "Account or password is incorrect" };
                dialog.Sounds();
                dialog.time = 2000;
                tbUser.Text = "";
                tbPass.Password = "";
                dialog.Owner = Window.GetWindow(this);
                dialog.ShowDialog();
                return;
            }
            
            
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            var screen = new SettingScrenn();
            screen.ShowDialog();
        }
    }
}
