using BusinessObject.Models;
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
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static Member _user = new Member();
        // <ProductId, Quantity>
        Dictionary<int, int> cart;

        public UserWindow(Member user)
        {
            InitializeComponent();
            _user = user;
            cart = new Dictionary<int, int>();
        }

        private void BtnViewProductClicked(object sender, RoutedEventArgs e)
        {
           
            UserProductWindow productWindow = new UserProductWindow(cart);
            productWindow.ShowDialog();
        }

        private void BtnOrderClicked(object sender, RoutedEventArgs e)
        {
            UserOrderWindow userOrderWindow = new UserOrderWindow(cart);
            userOrderWindow.ShowDialog();
        }

        private void BtnOrderHistoryClicked(object sender, RoutedEventArgs e)
        {
            UserHistoryOrdersWindow userHistoryOrdersWindow = new UserHistoryOrdersWindow();
            userHistoryOrdersWindow.Show();
        }

        private void BtnLogoutClicked(object sender, RoutedEventArgs e)
        {
            WindowLogin windowLogin = new WindowLogin();
            Close();
            windowLogin.Show();
        }

        private void BtnUserUpdateClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateMemberModalWindow updateMemberModalWindow = new UpdateMemberModalWindow(_user, null);
                updateMemberModalWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
    }
}
