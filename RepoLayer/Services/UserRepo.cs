using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepoLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;
       
        public UserRepo(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;

            
        }
        public UserEntity UserReg(UserRegModel model)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Email = model.Email;
                userEntity.DateOfBirth = model.DateOfBirth;
                userEntity.Password = model.Password;
                
                fundooContext.Users.Add(userEntity);

                fundooContext.SaveChanges();

                if (userEntity != null)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        public string UserLogin(UserLoginModel loginModel)
        {
            try
            {
                var userEntity = fundooContext.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.Password == loginModel.Password);


                if (userEntity != null)
                {
                    var token = GenerateJwtToken(userEntity.Email, userEntity.UserId);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public string GenerateJwtToken(string Email, long UserId)
        {

            var claims = new List<Claim>
            {
            new Claim("UserId", UserId.ToString()),
            new Claim("Email", Email),
                // Add any other claims you want to include in the token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["JwtSettings:Issuer"], configuration["JwtSettings:Audience"], claims, DateTime.Now, DateTime.Now.AddHours(1), creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string ForgotPassword(string email,string newPassword, string confirmPassword)
        {
            try
            {
                var emailValidity = fundooContext.Users.FirstOrDefault(u => u.Email == email);
                if (emailValidity != null)
                {
                   if(newPassword == confirmPassword)
                   {
                        emailValidity.Password = confirmPassword;
                        fundooContext.Users.Update(emailValidity);
                        fundooContext.SaveChanges();
                        return emailValidity.Password;
                   }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
   
}
