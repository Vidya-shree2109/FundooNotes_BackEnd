using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepoLayer.Services
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;
        private readonly IConfiguration config;
        public UserRL(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;
            this.config = config;
        }
        public UserEntity Registration(UserRegistration user)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Email = user.Email;
                userEntity.FirstName = user.FirstName;
                userEntity.LastName = user.LastName;
                userEntity.Password = user.Password;
                fundooContext.Users.Add(userEntity);
                int result=fundooContext.SaveChanges();
                if(result > 0)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string JwtMethod(string email, long id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.config[("Jwt:key")]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimTypes.Email, email),
                        new Claim("id", id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                tokenKey, SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
        public string Login(UserLogin userLogin)
        {
            try
            {
                var login = fundooContext.Users.SingleOrDefault(x => x.Email == userLogin.Email && x.Password == userLogin.Password);

                if (login != null)
                {
                    var token = JwtMethod(login.Email, login.UserId);
                    return token;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
