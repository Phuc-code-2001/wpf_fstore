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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMemberClicked(object sender, RoutedEventArgs e)
        {
            WindowMember windowMember = new WindowMember();
            windowMember.ShowDialog();
        }

        private void BtnProductClicked(object sender, RoutedEventArgs e)
        {
            WindowProduct windowProduct = new WindowProduct();
            windowProduct.ShowDialog();
        }

        private void BtnOrderClicked(object sender, RoutedEventArgs e)
        {
            WindowOrder windowOrder = new WindowOrder();
            windowOrder.ShowDialog();
        }

        private void BtnLogoutClicked(object sender, RoutedEventArgs e)
        {
            WindowLogin windowLogin = new WindowLogin();
            Close();
            windowLogin.Show();
        }
    }
}
