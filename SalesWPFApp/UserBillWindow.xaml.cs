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
    /// Interaction logic for UserBillWindow.xaml
    /// </summary>
    public partial class UserBillWindow : Window
    {

        int orderId;

        Member user;
        Order order;
        double discount = 0;
        List<OrderDetail> orderDetails;

        public UserBillWindow(int _orderId)
        {
            InitializeComponent();
            orderId = _orderId;
            order = OrderDAO.Instance.GetOrderByID(_orderId);
            user = MemberDAO.Instance.GetMemberByID(order.MemberId); 
            orderDetails = OrderDetailDAO.Instance.GetOrderDetailList().Where(od => od.OrderId == orderId).ToList();

            LoadDataContext();
            LoadOrderDetails();
        }

        private void LoadDataContext()
        {
            if(orderDetails.Count != 0)
            {
                discount = orderDetails[0].Discount;
            }

            var orderInfoNeeded = new
            {
                Email=user.Email,
                OrderId=orderId,
                OrderDate=order.OrderDate.ToShortDateString(),
                RequiredDate=order.RequiredDate?.ToShortDateString(),
                ShippedDate=order.ShippedDate?.ToShortDateString(),
                Freight=(double) (order.Freight ?? 0),
                Discount=(int) (discount * 100),
            };

            DataContext = orderInfoNeeded;
        }

        private void LoadOrderDetails()
        {
            decimal totalCost = 0;
            decimal totalPayment = 0;

            var orderDeatailsProcessed = orderDetails.Select(item =>
            {
                var cost = item.UnitPrice * item.Quantity;
                totalCost += cost;
                return new
                {
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice =(double) item.UnitPrice,
                    Cost = (double) cost,
                };
            }).ToList();

            totalCost = totalCost * (decimal) (1 - discount);
            totalPayment = totalCost + (order.Freight ?? 0);

            dgDetails.ItemsSource = orderDeatailsProcessed;

            txtTotalCost.Text = ((double)totalCost).ToString();
            txtTotalPayment.Text = ((double)totalPayment).ToString();

        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
