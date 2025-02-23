using Api.Data;
using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        public UserController(IUserRepository userRepository,DataContext context) {
            _userRepository = userRepository;
            _context = context;
        }
        [HttpPost("Post")]
        public IActionResult CreateUser(UserDto user)
        {
            try
            {
                var result = _userRepository.CreateUser(user);
                return Ok(result);  // Trả về 200 OK nếu tạo thành công
            }
            catch (InvalidOperationException ex)
            {
                // Trả về 409 Conflict nếu tài khoản đã tồn tại
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Trả về lỗi khác nếu có
                return StatusCode(500, new { message = "Đã có lỗi xảy ra", error = ex.Message });
            }
        }
        [HttpDelete]
        public IActionResult DeleteUser(int id) {
            return Ok( _userRepository.Delete(id));
        }
        [HttpGet]
        public IActionResult GetAll() {
            return Ok( _userRepository.GetAll());
        }
        [HttpGet("Id")]
        public IActionResult Get(int id) {
             return Ok(_userRepository.GetById(id));
        }
        [HttpGet("Name")]
        public IActionResult GetByName(string name) {
            return Ok(_userRepository.GetByName(name));
        }
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string email, string oldPass,  [FromQuery] UpdateUserDto user) {
            return Ok(_userRepository.ResetPassword(user, oldPass, email));
        }
        [HttpGet("Login")]
        public IActionResult Login(string userName, string password)
        {
            return Ok(_userRepository.Login(userName,password));
        }
    }
}
