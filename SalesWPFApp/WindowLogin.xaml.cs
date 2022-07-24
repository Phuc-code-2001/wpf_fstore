using BusinessObject.Models;
using DataAccess;
using Microsoft.Extensions.Configuration;
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
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {

        public WindowLogin()
        {
            InitializeComponent();
        }

        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPwd.Password;
            
            try
            {
                Authenticate(email, password);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void Authenticate(string email, string password)
        {
            try
            {
                if (email == String.Empty || password == String.Empty)
                {
                    MessageBox.Show("Email or Password can't be empty.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var root = builder.Build();

                var provider = root.Providers.FirstOrDefault();
                string emailAdmin = String.Empty;
                string pwdAdmin = String.Empty;
                if(provider != null)
                {
                    provider.TryGet("Admin:Email", out emailAdmin);
                    provider.TryGet("Admin:Password", out pwdAdmin);
                }

                if (email.Equals(emailAdmin) && password.Equals(pwdAdmin))
                {
                    // Đăng nhập admin thành công
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.Show();
                }
                else
                {
                    Member member = MemberDAO.Instance.Authenticate(email, password);
                    UserWindow userWindow = new UserWindow(member);
                    Close();
                    userWindow.Show();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
