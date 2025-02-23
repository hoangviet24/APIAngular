using Api.Data;
using Api.Migrations;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavController : ControllerBase
    {
        private readonly DataContext _context;
        public FavController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("Post")]
        public IActionResult Post(AddFavDto favDto)
        {
            var fav = new Models.Favorite
            {
                UserId = favDto.UserId,
                AnimalId = favDto.AnimalId,
            };
            _context.Favorites.Add(fav);
            _context.SaveChanges();
            return Ok("Đã thêm thành công");
        }
        [HttpGet("GetAll")]
        public IActionResult Get(int Uid) {
            var getUid = _context.Favorites.Where(x => x.Id == Uid);
            var get = getUid.Select(x => new FavDto
            {
                Id = x.Id,
                UserId = x.UserId,
                AnimalId=x.AnimalId,    
            }).ToList();
            return Ok(get);
        }
    }
}
