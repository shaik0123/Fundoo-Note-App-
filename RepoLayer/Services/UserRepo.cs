using CommonLayer.Models;
using FundooNoteSub.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly RabbitMQPublisher rabbitMQPublisher;
       
        public UserRepo(FundooContext fundooContext, IConfiguration configuration, RabbitMQPublisher rabbitMQPublisher)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
            this.rabbitMQPublisher = rabbitMQPublisher;

            
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
                    var message = new UserRegMsg { Email = userEntity.Email };
                    var messageJson = JsonConvert.SerializeObject(message);
                    rabbitMQPublisher.PublishMessage("User-Registration-Queue", messageJson);
                    // Example of sending a message to the RabbitMQ queue
                    // Print a message to the console to verify
                    Console.WriteLine($"Message sent to queue: {messageJson}");
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
            new Claim(ClaimTypes.Email, Email),
                // Add any other claims you want to include in the token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["JwtSettings:Issuer"], configuration["JwtSettings:Audience"], claims, DateTime.Now, DateTime.Now.AddHours(1), creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                var emailValidity = fundooContext.Users.FirstOrDefault(u => u.Email == forgotPasswordModel.Email);
                if (emailValidity != null)
                {
                    var token = GenerateJwtToken(emailValidity.Email, emailValidity.UserId);
                    MSMQ msmq = new MSMQ();
                    msmq.sendData2Queue(token);


                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool ResetPassword(string email, string newpassword, string confirmPassword)
        {
            try
            {
                if (newpassword == confirmPassword)
                {
                    var isEmailPresent = fundooContext.Users.FirstOrDefault(u => u.Email == email);

                    if (isEmailPresent != null)
                    {

                        isEmailPresent.Password = confirmPassword;
                        fundooContext.Users.Update(isEmailPresent);
                        fundooContext.SaveChanges();
                        return true;
                    }
                }

                return false;


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
   
}
