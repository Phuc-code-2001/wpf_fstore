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
    /// Interaction logic for CreateMemberModalWindow.xaml
    /// </summary>
    public partial class CreateMemberModalWindow : Window
    {

        WindowMember _windowMember;
        Member member;

        public CreateMemberModalWindow(WindowMember windowMember)
        {
            InitializeComponent();
            member = new Member();
            DataContext = member;
            _windowMember = windowMember;
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddNewClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                member.Password = txtPwd.Password;
                member.TryValidate();
                bool emailExisted = MemberDAO.Instance.GetMemberByEmail(member.Email) != null;
                if (emailExisted)
                {
                    throw new Exception("Email already exist!");
                }
                MemberDAO.Instance.AddNew(member);
                MessageBox.Show("Add new member success.", "Add New Member", MessageBoxButton.OK, MessageBoxImage.Information);
                _windowMember.LoadMemberList();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add New Member", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
