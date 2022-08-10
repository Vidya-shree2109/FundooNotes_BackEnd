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
                userEntity.Password = EncryptPassword(user.Password);
                this.fundooContext.Users.Add(userEntity);
                int result=this.fundooContext.SaveChanges();
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
        public string EncryptPassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string DecryptPassword(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
        public string Login(UserLogin userLogin)
        {
            try
            {
                var login = fundooContext.Users.Where(x => x.Email == userLogin.Email).FirstOrDefault();
                if (login != null && DecryptPassword(login.Password) == userLogin.Password)
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
        public string ForgotPassword(string email)
        {
            try
            {
                var emailCheck = fundooContext.Users.FirstOrDefault(x => x.Email == email);
                if (emailCheck != null)
                {
                    var token = JwtMethod(emailCheck.Email, emailCheck.UserId);
                    new MsmqModel().MsmqSend(token);
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

        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                if(password.Equals(confirmPassword))
                {
                    UserEntity user = fundooContext.Users.Where(e => e.Email == email).FirstOrDefault();
                    user.Password = confirmPassword;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
