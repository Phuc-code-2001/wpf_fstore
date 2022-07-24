using BusinessObject.Models;
using DataAccess;
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
    /// Interaction logic for UserHistoryOrdersWindow.xaml
    /// </summary>
    public partial class UserHistoryOrdersWindow : Window
    {

        Member user;
        
        public UserHistoryOrdersWindow()
        {
            InitializeComponent();
            user = UserWindow._user;
            LoadOrderList();
        }

        private void LoadOrderList()
        {
            IEnumerable<Order> orders = OrderDAO.Instance.GetOrderList().Where(o => o.MemberId == user.MemberId);

            var orderListProcessed = orders.Select(order =>
            {
                var orderDetails = OrderDetailDAO.Instance.GetOrderDetailList().Where(od => od.OrderId == order.OrderId);
                decimal totalPayment = 0;
                foreach(var od in orderDetails)
                {
                    totalPayment += (od.Quantity * od.UnitPrice) * (decimal) (1 - od.Discount);
                }

                totalPayment += order.Freight ?? 0 ;

                return new 
                {
                    OrderId=order.OrderId,
                    OrderDate=order.OrderDate.ToShortDateString(),
                    TotalPayment=(double)totalPayment,
                };
            });

            dgOrders.ItemsSource = orderListProcessed;
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {

        }

        private void ViewOrderDetailsClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int OrderId = (int) ((Button)sender).Tag;
                UserBillWindow userBillWindow = new UserBillWindow(OrderId);
                userBillWindow.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Lịch sử đơn hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
