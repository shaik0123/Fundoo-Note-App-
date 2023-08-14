using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        public UserController(IUserBusiness userBusiness) 
        {
            this._userBusiness = userBusiness;
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult UserRegModel(UserRegModel model) 
        {
            var result = _userBusiness.UserReg(model);
            if (result != null)
            {
                return Ok(new  { success = true, messege = "User Registration Successful",data = result });
            }
            else
            {
                return BadRequest(new { success = false, messege = "User Registration UnSuccessful", data = result });
            }

        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLoginModel loginModel)
        {
            var result = _userBusiness.UserLogin(loginModel);
            if (result != null)
            {
                return Ok(new { success = true, messege = "User Login Successfull", data = result });
            }
            else
            {
                return Unauthorized(new { messeg = "Password incorrect " });

            }

        }
    }
}
