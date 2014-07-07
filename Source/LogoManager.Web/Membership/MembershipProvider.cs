using ChannelManager.EF;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace ChannelManager
{
    public class MembershipProvider : System.Web.Security.MembershipProvider
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        Random random = new Random();

        string GetMD5Hash(string text)
        {
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        string GetRandomLetters(int amount)
        {    
            var sb = new StringBuilder(amount);
            for (int i = 0; i < amount; i++) sb.Append(Encoding.ASCII.GetString(new byte[] { (byte)random.Next('A', 'Z') }));
            return sb.ToString();
        }

        public override string ApplicationName
        {
            get { return "ChannelManager"; }
            set { }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (var dc = new RepositoryContext("LogoDB"))
            {
                var user = dc.Users.FirstOrDefault(u => u.Login == username);
                if (user != null && user.Password == GetMD5Hash(oldPassword))
                {
                    user.Password = GetMD5Hash(newPassword);
                    dc.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            using (var ctx = new RepositoryContext("LogoDB"))
            {
                if (ctx.Users.Any(u => u.Login == username))
                    status = MembershipCreateStatus.DuplicateUserName;
                else if (ctx.Users.Any(u => u.Email == email))
                    status = MembershipCreateStatus.DuplicateEmail;
                else
                {
                    var user = new User() { Id = Guid.NewGuid(), Login = username, Email = email, Password = GetMD5Hash(password), Repository = ctx.Repositorys.First() };
                    if (ctx.Users.Count() == 0)
                    {
                        user.Roles.Add(ctx.Roles.First(r => r.Name == "Administrator"));
                    }
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                    status = MembershipCreateStatus.Success;
                    return new MembershipUser(this.Name, username, user.Id, email, null, null, false, false, DateTime.Now, DateTime.MinValue, DateTime.Now, DateTime.Now, DateTime.MinValue);
                }
                return null;
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (var dc = new RepositoryContext("LogoDB"))
            {
                var user = dc.Users.FirstOrDefault(u => u.Login == username);
                if (user != null)
                    return new MembershipUser(this.Name, user.Login, user.Id, user.Email, null, null, false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (var dc = new RepositoryContext("LogoDB"))
            {
                var user = dc.Users.FirstOrDefault(u => u.Id == (Guid)providerUserKey);
                if (user != null)
                    return new MembershipUser(this.Name, user.Login, user.Id, user.Email, null, null, false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            using (var dc = new RepositoryContext("LogoDB"))
            {
                var user = dc.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                    return user.Login;
            }
            return null;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 15; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return "+*"; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            using (var dc = new RepositoryContext("LogoDB"))
            {
                var user = dc.Users.FirstOrDefault(u => u.Login == username);
                if (user != null)
                {
                    var newPw = GetRandomLetters(6);
                    user.Password = GetMD5Hash(newPw);
                    dc.SaveChanges();
                    return newPw;
                }
            }
            return null;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            string md5Hash = GetMD5Hash(password);
            using (var dc = new RepositoryContext("LogoDB"))
            {
                return dc.Users.Any(u => u.Login == username && u.Password == md5Hash);
            }
        }
    }
}