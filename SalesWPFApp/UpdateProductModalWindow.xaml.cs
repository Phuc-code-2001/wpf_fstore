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
    /// Interaction logic for UpdateProductModalWindow.xaml
    /// </summary>
    public partial class UpdateProductModalWindow : Window
    {
        WindowProduct _windowProduct;

        public UpdateProductModalWindow(WindowProduct windowProduct, Product product)
        {
            InitializeComponent();
            _windowProduct = windowProduct;
            product = ProductDAO.Instance.GetProductByID(product.ProductId);
            DataContext = product;
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Product editedProduct = (Product)DataContext;
                ProductDAO.Instance.Update(editedProduct);
                MessageBox.Show("Update product success.", "Update Product", MessageBoxButton.OK, MessageBoxImage.Information);
                _windowProduct.LoadProducts();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Product", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
