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
    /// Interaction logic for UserProductWindow.xaml
    /// </summary>
    public partial class UserProductWindow : Window
    {
        IEnumerable<Product> Products;
        Dictionary<int, int> tempCart;

        public UserProductWindow(Dictionary<int, int> userCart)
        {
            InitializeComponent();
            tempCart = userCart;
            LoadCart();
            Products = ProductDAO.Instance.GetProductList();
            Refresh();
        }

        private void LoadCart()
        {
            int sumOfNumberOfProducts = 0;
            foreach(var cartPair in tempCart)
            {
                sumOfNumberOfProducts += cartPair.Value;
            }
            DataContext = new
            {
                SumOfNumberOfProducts = $"Giỏ hàng ({sumOfNumberOfProducts})"
            };
        }
        
        public void LoadProducts(IEnumerable<Product> products)
        {
            dgProducts.ItemsSource = products;
        }

        private void SearchClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim().ToLower();
                List<Product> seachedProducts = new List<Product>();
                seachedProducts.AddRange(Products.Where(p => p.ProductName.ToLower().Contains(searchText)).ToList());
                seachedProducts.AddRange(Products.Where(p => p.ProductId.ToString().Contains(searchText)).ToList());
                seachedProducts.AddRange(Products.Where(p => p.Weight.ToLower().Contains(searchText)).ToList());
                seachedProducts.AddRange(Products.Where(p => p.UnitPrice.ToString().ToLower().Contains(searchText)).ToList());
                seachedProducts.AddRange(Products.Where(p => p.UnitslnStock.ToString().ToLower().Contains(searchText)).ToList());

                LoadProducts(seachedProducts.ToHashSet());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Product", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        private void Refresh()
        {
            LoadProducts(Products);
            LoadCart();
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            Products = ProductDAO.Instance.GetProductList();
            Refresh();
        }

        private void CartClicked(object sender, RoutedEventArgs e)
        {
            UserOrderWindow userOrderWindow = new UserOrderWindow(tempCart);
            userOrderWindow.ShowDialog();
        }

        private void AddToCartClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = (int) ((Button)sender).Tag;
                try
                {
                    int quantity = tempCart[productId];
                    tempCart[productId] = quantity + 1;
                }
                catch(KeyNotFoundException ex)
                {
                    tempCart.Add(productId, 1);
                }
                LoadCart();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
