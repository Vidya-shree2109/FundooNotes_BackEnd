using CommonLayer.Model;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface IUserRL
    {
        public UserEntity Registration(UserRegistration user);
        string Login(UserLogin userLogin);
    }
}
