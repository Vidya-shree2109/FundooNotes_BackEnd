using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
