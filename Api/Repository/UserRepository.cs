using Api.Data;
using Api.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                if (getName == null)
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    var userEntity = new User()
                    {
                        UserName = user.UserName,
                        Password = hashedPassword,
                    };

                    _context.Users.Add(userEntity);
                    _context.SaveChanges();

                }
                else
                {
                    throw new InvalidOperationException("Tài khoản đã tồn tại");
                }
         

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

        //update user
        //b1: Người dùng nhập email và password, để tìm xem có tài khoản không
        //b2: Nếu có sẽ bắt đầu nhập mật khẩu mới, và lưu vào db
        public UpdateUserDto ResetPassword(UpdateUserDto user, string oldPass, string email)
        {
            // Tìm user trong database theo email
            var userEntity = _context.Users.FirstOrDefault(x => x.UserName == email);

            if (userEntity != null && BCrypt.Net.BCrypt.Verify(oldPass, userEntity.Password))
            {
                // Mã hóa mật khẩu mới
                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Cập nhật user vào database
                _context.Users.Update(userEntity);
                _context.SaveChanges();

                return user;
            }

            return null; // Sai mật khẩu hoặc user không tồn tại
        }
    }
}
