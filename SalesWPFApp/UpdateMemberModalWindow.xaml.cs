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
    /// Interaction logic for UpdateMemberModalWindow.xaml
    /// </summary>
    public partial class UpdateMemberModalWindow : Window
    {
        WindowMember? _windowMember;

        public UpdateMemberModalWindow(Member member, WindowMember? windowMember)
        {
            InitializeComponent();
            _windowMember = windowMember;
            member = MemberDAO.Instance.GetMemberByID(member.MemberId);
            DataContext = member;
            txtPwd.Password = member.Password;
        }

        private void UpdateClicked(object sender, RoutedEventArgs e)
        {
            Member editedMember = (Member) DataContext;
            editedMember.Password = txtPwd.Password;
            try
            {
                editedMember.TryValidate();
                bool emailExisted = MemberDAO.Instance.GetMemberByEmail(editedMember.Email) != null 
                    && MemberDAO.Instance.GetMemberByEmail(editedMember.Email).MemberId != editedMember.MemberId;
                if(emailExisted)
                {
                    throw new Exception("Email already exist!");
                }

                MemberDAO.Instance.Update(editedMember);
                MessageBox.Show("Update member success.", "Update Member", MessageBoxButton.OK, MessageBoxImage.Information);
                if(_windowMember != null)_windowMember.LoadMemberList();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Member", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
