using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPatch]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var result = _userBusiness.ForgotPassword(forgotPasswordModel);
            if (result != null)
            {
                return Ok(new {  messege = "Token Sent Success", });
            }
            else
            {
                return Unauthorized(new { messeg = "Credentials not exist" });

            }

        }
        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]

        public IActionResult ResetPassword(string newPassword, string ConfirmPassword)

        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            if (email != null)
            {
                var result = _userBusiness.ResetPassword(email, newPassword, ConfirmPassword);
                if (result == true)
                {
                    return Ok(new { success = true, message = "Reset Password  Successfully" });
                }
                else
                {
                    return Unauthorized(new { success = false, message = "Invalid Credentials Reset Password  UnSuccessfully " });
                }

            }
            return null;
        }
    }
}
