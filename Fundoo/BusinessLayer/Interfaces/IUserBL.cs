using CommonLayer.Model;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        public UserEntity Registration(UserRegistration user);
        public string Login(UserLogin userLogin);
        public string ForgotPassword(string email);
        public bool ResetPassword(string email, string password, string confirmPassword);
    }
}
