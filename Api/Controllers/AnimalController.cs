using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animal;
        public AnimalController(IAnimalRepository animal) {
            _animal = animal;
        }

        [HttpGet("GetAll")]
        public IActionResult Filtering([FromQuery] string? name,int page =1, int pageSize = 10)
        {
            try
            {
                var totalCount = _animal.GetAnimalByName(name).Count();
                var AnimalPerPage = _animal.GetAnimalByName(name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return Ok(AnimalPerPage);
            }
            catch
            {
                var totalCount = _animal.GetAllAnimals().Count();
                var AnimalPerPage = _animal.GetAllAnimals()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return Ok(AnimalPerPage);
            }
        }
        [HttpGet("GetTotal")]
        public IActionResult GetTotal() {
            return Ok(_animal.GetAllAnimals().Count());
        }
        [HttpPost("Post-file")]
        public IActionResult UploadFile(IFormFile file) {
            return Ok( new UploadHandle().Upload(file));
        }
        [HttpGet("view-file")]
        public IActionResult ViewFile(string? fileName)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(uploadPath))
                return NotFound("Uploads directory does not exist.");

            if (!string.IsNullOrEmpty(fileName))
            {
                var filePath = Path.Combine(uploadPath, fileName);
                if (!System.IO.File.Exists(filePath))
                    return NotFound("File does not exist.");

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = "image/" + Path.GetExtension(fileName).TrimStart('.');
                return File(fileBytes, contentType);
            }

            // Trả danh sách file nếu không có fileName
            var files = Directory.GetFiles(uploadPath).Select(Path.GetFileName).ToList();
            return Ok(files);
        }
        [HttpDelete("delete/{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            System.IO.File.Delete(filePath);  // Xóa file

            return Ok(new { message = "File deleted successfully." });
        }

        [HttpPost("Add")]
        public IActionResult Post([FromBody] AddAnimalDto animal) {
            var post = _animal.AddAnimal(animal);
            return Ok(post);
        }
        [HttpPut ("Update")]
        public IActionResult Put( int id, [FromBody] AddAnimalDto animal)
        {
            var put = _animal.PutAnimal(id,animal);
            return Ok(put);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int id) {
            var delete = _animal.Delete(id);
            return Ok(delete);
        }
        [HttpGet("Type")]
        public IActionResult GetType(string type) {
            return Ok(_animal.GetAnimalByType(type));
        }
    }
}
