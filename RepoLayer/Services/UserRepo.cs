using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext fundooContext;
       
        public UserRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
            
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
    }
}
