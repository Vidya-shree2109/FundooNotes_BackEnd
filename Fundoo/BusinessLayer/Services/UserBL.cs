using BusinessLayer.Interfaces;
using CommonLayer.Model;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL= userRL;
        }
        public UserEntity Registration(UserRegistration user)
        {
            try
            {
                return userRL.Registration(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Login(UserLogin userLogin)
        {
            try
            {
                return this.userRL.Login(userLogin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
