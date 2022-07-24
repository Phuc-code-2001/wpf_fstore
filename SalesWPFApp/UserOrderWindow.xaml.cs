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
    /// Interaction logic for UserOrderWindow.xaml
    /// </summary>
    public partial class UserOrderWindow : Window
    {
        Dictionary<int, int> tempCart;
        List<OrderDetail> OrderDetails;

        double discount = 0;

        // Phí vận chuyển
        double freight = 25000;
        

        public UserOrderWindow(Dictionary<int, int> userCart)
        {
            InitializeComponent();
            tempCart = userCart;
            OrderDetails = new List<OrderDetail>();
            LoadOrderDetails();
            DataContext = new
            {
                Freight = freight.ToString() + "đ",
            };
            
        }

        public void ResetCart()
        {
            tempCart.Clear();
            LoadOrderDetails();
        }

        public void checkCartEmpty()
        {
            if (tempCart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng chọn hàng để mua trước.", "Order", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void genOrderDetails()
        {
            OrderDetails = new List<OrderDetail>();
            foreach(var transaction in tempCart)
            {
                int productId = transaction.Key;
                int quantity  = transaction.Value;
                Product product = ProductDAO.Instance.GetProductByID(productId);
                
                OrderDetail detail = new OrderDetail()
                {
                    ProductId = productId,
                    Product = product,
                    Quantity = quantity,
                    UnitPrice = product.UnitPrice
                };

                OrderDetails.Add(detail);
            }
        }

        private void LoadOrderDetails()
        {
            genOrderDetails();
            dgOrder.ItemsSource = OrderDetails;
            LoadTotalPaymentCost();
        }

        
        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            LoadOrderDetails();
        }

        private void RemoveProductClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = (int)((Button)sender).Tag;
                if(tempCart.ContainsKey(productId))
                {
                    tempCart.Remove(productId);
                    LoadOrderDetails();
                }
                checkCartEmpty();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DiscountSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDiscount = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            if(selectedDiscount != null)
            {
                discount = double.Parse(selectedDiscount.ToString() ?? "0");
            }
            LoadTotalPaymentCost();
        }

        private void LoadTotalPaymentCost()
        {
            try
            {
                double total = 0;
                foreach(var orderDetail in OrderDetails)
                {
                    total += (double) orderDetail.UnitPrice * orderDetail.Quantity;
                }
                txtTotalPaymentCost.Text = (total * (1 - discount) + freight).ToString() + "đ";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConfirmClicked(object sender, RoutedEventArgs e)
        {
            checkCartEmpty();
            if(tempCart.Count != 0)
            {
                // Tạo các đối tượng Order, OrderDetail bỏ vào database
                try
                {
                    Order order = new Order()
                    {
                        MemberId = UserWindow._user.MemberId,
                        OrderDate = DateTime.Now.Date,
                        RequiredDate = DateTime.Now.Date + TimeSpan.FromDays(7),
                        ShippedDate = DateTime.Now.Date + TimeSpan.FromDays(1),
                        Freight = (decimal) freight
                    };

                    // Transaction create orderDetails
                    foreach (var orderDetail in OrderDetails)
                    {
                        if(orderDetail.Quantity > orderDetail.Product?.UnitslnStock)
                        {
                            throw new Exception($"Mặt hàng '{orderDetail.Product?.ProductName}' không đủ số lượng.\nChỉ còn '{orderDetail.Product?.UnitslnStock}'");
                        }

                        orderDetail.Discount = discount;
                        // Ko set lại null thì bị INSERT_INDENTITY column not set to be ON...
                        orderDetail.Product = null;
                        order.OrderDetails.Add(orderDetail);
                    }

                    try
                    {
                        OrderDAO.Instance.AddNew(order);
                        MessageBox.Show("Đặt thành công. Ok để xem thông tin đơn hàng.");
                        ResetCart();
                        ShowBill(order.OrderId);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Lỗi hệ thống. Đặt hàng ko thành công.");
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void IncreaseQuantityClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = (int)((Button)sender).Tag;
                if(tempCart.ContainsKey(productId))
                {
                    tempCart[productId] += 1;
                    LoadOrderDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DecreaseQuantityClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = (int)((Button)sender).Tag;
                if (tempCart.ContainsKey(productId))
                {
                    if(tempCart[productId] > 1)
                    {
                        tempCart[productId] -= 1;
                    }
                    else
                    {
                        tempCart.Remove(productId);
                    }
                    LoadOrderDetails();
                    checkCartEmpty();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowBill(int orderId)
        {
            UserBillWindow userBillWindow = new UserBillWindow(orderId);
            userBillWindow.Show();
        }
    }
}
