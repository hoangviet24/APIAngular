using Api.Data;
using Api.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public List<User> GetByName(string Name)
        {
            var getName = _context.Users.Where(x => x.UserName.ToLower().Contains(Name.ToLower()));
            return getName.ToList();
        }

        public User Login(string user, string pass)
        {
            var UserName = _context.Users.FirstOrDefault(x=>x.UserName == user);
            if (UserName != null && BCrypt.Net.BCrypt.Verify(pass, UserName.Password)) 
            {
                return UserName;   
            }
            return null;
        }

        UserDto IUserRepository.CreateUser(UserDto user)
        {
            try
            {
                var getName = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);
                if (getName != null)
                {
                    throw new Exception("Username đã tồn tại");
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userEntity = new User()
                {
                    UserName = user.UserName,
                    Password = hashedPassword,
                };

                _context.Users.Add(userEntity);
                _context.SaveChanges();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }

        User IUserRepository.Delete(int id)
        {
            var delId = _context.Users.Find(id);
            if (delId != null)
            {
                _context.Users.Remove(delId);
                _context.SaveChanges();
            }
            return delId;
        }

        List<User> IUserRepository.GetAll()
        {
            return _context.Users.ToList();
        }

        User? IUserRepository.GetById(int id)
        {
            return _context.Users.Find(id);
        }

        UpdateUserDto IUserRepository.UpdateUser(UpdateUserDto user, int Id)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var getId = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (getId != null)
            {
                getId.Password = hashedPassword;
                _context.SaveChanges();
            }
            return user;
        }
    }
}
