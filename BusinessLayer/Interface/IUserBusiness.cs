﻿using CommonLayer.Models;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        public UserEntity UserReg(UserRegModel model);
        public string UserLogin(UserLoginModel loginModel);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        public bool ResetPassword(string email, string newpassword, string confirmPassword);



    }
}
