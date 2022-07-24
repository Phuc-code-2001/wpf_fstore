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
    /// Interaction logic for WindowMember.xaml
    /// </summary>
    public partial class WindowMember : Window
    {

        public WindowMember()
        {
            InitializeComponent();
            LoadMemberList();
        }

        public void LoadMemberList()
        {
            dgMembers.ItemsSource = MemberDAO.Instance.GetMemberList();
        }

        private void RegisterClicked(object sender, RoutedEventArgs e)
        {
            CreateMemberModalWindow createMemberModalWindow = new CreateMemberModalWindow(this);
            createMemberModalWindow.ShowDialog();
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            LoadMemberList();
        }

        private void UpdateClicked(object sender, RoutedEventArgs e)
        {
            Member selectedMember = (Member) dgMembers.SelectedItem;
            if(selectedMember != null)
            {
                UpdateMemberModalWindow updateMemberModalWindow = new UpdateMemberModalWindow(selectedMember, this);
                updateMemberModalWindow.ShowDialog();
            }
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            Member selectedMember = (Member)dgMembers.SelectedItem;
            if (selectedMember != null)
            {
                MessageBoxResult result = MessageBox.Show($"Delete member {selectedMember.MemberId} ({selectedMember.Email})?", "Delete Member", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    MemberDAO.Instance.Remove(selectedMember);
                    MessageBox.Show("Delete success.", "Delete Member", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMemberList();
                }
            }
        }
    }
}
