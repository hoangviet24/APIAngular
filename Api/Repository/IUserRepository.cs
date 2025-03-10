﻿using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetById(int id);
        List<User> GetByName(string Name);
        UserDto CreateUser(UserDto user);
        UpdateUserDto ResetPassword(UpdateUserDto user,string oldPass, string email);
        User Login(string user, string pass);
        User Delete(int id);
    }
}
