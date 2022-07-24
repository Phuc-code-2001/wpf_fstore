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
    /// Interaction logic for WindowOrder.xaml
    /// </summary>
    public partial class WindowOrder : Window
    {
        public WindowOrder()
        {
            InitializeComponent();
            LoadOrderList();
        }

        private void LoadOrderList()
        {
            var orders = OrderDAO.Instance.GetOrderList();
            var orderListProcessed = orders.Select(order =>
            {
                var orderDetails = OrderDetailDAO.Instance.GetOrderDetailList().Where(od => od.OrderId == order.OrderId);
                var member = MemberDAO.Instance.GetMemberByID(order.MemberId);

                decimal totalPayment = 0;
                int count = 0;
                foreach (var od in orderDetails)
                {
                    count += od.Quantity;
                    totalPayment += (od.Quantity * od.UnitPrice) * (decimal)(1 - od.Discount);
                }

                totalPayment += order.Freight ?? 0;
                

                return new
                {
                    OrderId = order.OrderId,
                    Email = member.Email,
                    OrderDate = order.OrderDate.ToShortDateString(),
                    Count = count,
                    TotalPayment = (double)totalPayment,
                };

            });

            dgOrders.ItemsSource = orderListProcessed;
            Statistics(orders.ToList());
        }

        private void ViewOrderDetailsClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int OrderId = (int)((Button)sender).Tag;
                UserBillWindow userBillWindow = new UserBillWindow(OrderId);
                userBillWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quản lý đơn hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int OrderId = (int)((Button)sender).Tag;
                MessageBoxResult result = MessageBox.Show($"Delete member {OrderId} ?", "Delete Order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Order order = OrderDAO.Instance.GetOrderByID(OrderId);
                    OrderDAO.Instance.Remove(order);
                    MessageBox.Show("Xóa đơn hàng thành công.", "Quản lý đơn hàng", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadOrderList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quản lý đơn hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReportClicked(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpStart.SelectedDate;
            DateTime? endDate = dpEnd.SelectedDate;

            if(startDate.HasValue && endDate.HasValue)
            {
                var orders = OrderDAO.Instance.GetOrderList().Where(o => startDate <= o.OrderDate && o.OrderDate <= endDate).ToList();
                Statistics(orders);
            }
            else
            {
                MessageBox.Show("Chọn ngày bắt đầu và kết thúc!", "Thống kê", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Statistics(List<Order> orders)
        {
            Dictionary<int, int> ProductCounter = new Dictionary<int, int>();
            int totalProductCount = 0;
            int orderCount = orders.Count;
            foreach(Order order in orders)
            {
                var orderDetails = OrderDetailDAO.Instance.GetOrderDetailList().Where(od => od.OrderId == order.OrderId);
                foreach(OrderDetail detail in orderDetails)
                {
                    totalProductCount += detail.Quantity;
                    if(ProductCounter.ContainsKey(detail.ProductId))
                    {
                        ProductCounter[detail.ProductId] += detail.Quantity;
                    }
                    else
                    {
                        ProductCounter[detail.ProductId] = detail.Quantity;
                    }
                }
            }

            int maxValue = 0;
            string bestsellerProductName = "";
            foreach(var productSeller in ProductCounter)
            {
                if(productSeller.Value > maxValue)
                {
                    Product product = ProductDAO.Instance.GetProductByID(productSeller.Key);
                    bestsellerProductName = product.ProductName;
                    maxValue = productSeller.Value;
                }
            }

            Report(orderCount, totalProductCount, bestsellerProductName);

        }

        private void Report(int OrderCount, int ProductCount, string BestSellerProductName)
        {

            DataContext = new
            {
                OrderCount = OrderCount, ProductCount = ProductCount, BestSellerProductName = BestSellerProductName
            };
        }
    }
}
