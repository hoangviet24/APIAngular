using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        [HttpPost]
        public IActionResult CreateUser(UserDto user)
        {
            return Ok( _userRepository.CreateUser(user));
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
        [HttpPut("Update")]
        public IActionResult Put(int Id,[FromQuery] UpdateUserDto user) {
            return Ok( _userRepository.UpdateUser(user, Id));
        }
        [HttpGet("Login")]
        public IActionResult Login(string userName, string password)
        {
            return Ok(_userRepository.Login(userName,password));
        }
    }
}
