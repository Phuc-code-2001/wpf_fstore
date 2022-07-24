using System;
using System.Collections.Generic;
using System.Net.Mail;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Member
    {
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        public int MemberId { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public void TryValidate()
        {
            Email = Email.Trim();
            if (Email == null)
            {
                throw new Exception("Missing Email!");
            }

            try
            {
                MailAddress email = new MailAddress(Email);
            }
            catch (Exception ex)
            {
                throw new Exception("Email invalid.");
            }

            Password = Password.Trim();
            if (Password.Contains(" "))
            {
                throw new Exception("Password must be not contain space character.");
            }

            if (Password.Length < 6)
            {
                throw new Exception("Password length must be greater than or equal 6.");
            }

            CompanyName = CompanyName.Trim();
            if (CompanyName == null)
            {
                throw new Exception("Missing CompanyName!");
            }

            City = City.Trim();
            if (City == null)
            {
                throw new Exception("Missing City!");
            }

            Country = Country.Trim();
            if (Country == null)
            {
                throw new Exception("Missing Country!");
            }

        }
    }
}
