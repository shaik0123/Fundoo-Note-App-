using BusinessLayer.Interface;
using CommonLayer.Models;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo _userRepo;
        public UserBusiness(IUserRepo userRepo) 
        {
            this._userRepo = userRepo;
        }
        public UserEntity UserReg(UserRegModel model)
        {
            try
            {
                return _userRepo.UserReg(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UserLogin(UserLoginModel loginModel)
      
        {
            try
            {
                return _userRepo.UserLogin(loginModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                return _userRepo.ForgotPassword(forgotPasswordModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool ResetPassword(string email, string newpassword, string confirmPassword)
        {
            try
            {
                return _userRepo.ResetPassword(email, newpassword, confirmPassword);
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
