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
    /// Interaction logic for WindowProduct.xaml
    /// </summary>
    public partial class WindowProduct : Window
    {
        public WindowProduct()
        {
            InitializeComponent();
            LoadProducts();
        }

        public void LoadProducts()
        {
            dgProducts.ItemsSource = ProductDAO.Instance.GetProductList();
        }

        private void CreateClicked(object sender, RoutedEventArgs e)
        {
            CreateProductModalWindow createProductModalWindow = new CreateProductModalWindow(this);
            createProductModalWindow.ShowDialog();
        }

        private void UpdateClicked(object sender, RoutedEventArgs e)
        {
            Product product = (Product)dgProducts.SelectedItem;
            if(product != null)
            {
                UpdateProductModalWindow updateProductModalWindow = new UpdateProductModalWindow(this, product);
                updateProductModalWindow.ShowDialog();
            }
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            Product product = (Product) dgProducts.SelectedItem;
            if(product != null)
            {
                MessageBoxResult result = MessageBox.Show($"Delete member {product.ProductId} ({product.ProductName})?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ProductDAO.Instance.Remove(product);
                    MessageBox.Show("Delete success.", "Delete Product", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts();
                }
            }
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
    }
}
