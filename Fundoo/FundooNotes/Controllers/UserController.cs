using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserBL iuserBL;
        public UserController(IUserBL iuserBL)
        {
            this.iuserBL = iuserBL;
        }
        [HttpPost("Register")]
        public IActionResult Registration(UserRegistration registration)
        {
            try
            {
                var result = iuserBL.Registration(registration);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Registration Successful",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "Registration Unsuccessful"
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                var result = iuserBL.Login(userLogin);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Login Successful",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "Login Unsuccessful"
                    });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost("Forget")]
        //[Route("api/Forget/{email}")]
        public IActionResult Forget(string email)
        {
            try
            {
                var token = iuserBL.ForgotPassword(email);
                if (token != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Reset mail sent successfully",
                        Response = token
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "Mail not sent"
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("Reset")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                var token = iuserBL.ResetPassword(email, password, confirmPassword);
                if (token != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Password has been changed successfully"
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "Unable to reset password, Please try again !"
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}