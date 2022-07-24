using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        public static readonly object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Member> GetMemberList()
        {
            List<Member> members;
            try
            {
                var FStoreDB = new FSTOREContext();
                members = FStoreDB.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;

        }

        public Member GetMemberByID(int memberID)
        {
            Member member = null;
            try
            {
                var FStoreDB = new FSTOREContext();
                member = FStoreDB.Members.SingleOrDefault(member => member.MemberId == memberID);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member GetMemberByEmail(string email)
        {
            Member member = null;
            try
            {
                var FStoreDB = new FSTOREContext();
                member = FStoreDB.Members.SingleOrDefault(member => member.Email == email);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void AddNew(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member == null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.Members.Add(member);
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member is already exist. ");
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member != null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.Entry<Member>(member).State = EntityState.Modified;
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member != null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.Members.Remove(member);
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member does not already exist. ");
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Member Authenticate(string email, string password)
        {
            try
            {
                var FStoreDB = new FSTOREContext();
                Member validateMember = FStoreDB.Members.SingleOrDefault(m => m.Email.Equals(email) && m.Password.Equals(password));
                if(validateMember != null) return validateMember;

                throw new Exception("The email or password inccorect. ");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
