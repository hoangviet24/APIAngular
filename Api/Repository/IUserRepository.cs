using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetById(int id);
        List<User> GetByName(string Name);
        UserDto CreateUser(UserDto user);
        UpdateUserDto UpdateUser(UpdateUserDto user,int Id);
        User Login(string user, string pass);
        User Delete(int id);
    }
}
