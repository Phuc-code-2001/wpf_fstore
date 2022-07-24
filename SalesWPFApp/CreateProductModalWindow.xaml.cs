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
    /// Interaction logic for CreateProductModalWindow.xaml
    /// </summary>
    public partial class CreateProductModalWindow : Window
    {
        WindowProduct _windowProduct;
        Product product;

        public CreateProductModalWindow(WindowProduct windowProduct)
        {
            InitializeComponent();
            _windowProduct = windowProduct;
            product = new Product();
            DataContext = product;
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddNewClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                product.ProductId = 0;
                product.TryValidate();
                ProductDAO.Instance.AddNew(product);
                MessageBox.Show("Add product success.", "Add New Product", MessageBoxButton.OK, MessageBoxImage.Information);
                _windowProduct.LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add New Product", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
