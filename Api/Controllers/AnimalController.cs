using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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

        [HttpGet("Random")]
        public IActionResult GetRandom()
        {
            return Ok(_animal.GetAllRandomAnimals(10));
        }
        [HttpGet("GetAll")]
        public IActionResult Filtering([FromQuery] string? name,int page =1, int pageSize = 10)
        {
            if(name != null)
            {
                var totalCount = _animal.GetAnimalByName(name).Count();
                var AnimalPerPage = _animal.GetAnimalByName(name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return Ok(AnimalPerPage);
            }
            else
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
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetId(int id) {
            return Ok(_animal.GetAnimalById(id));
        }
        [HttpPost("Post-file")]
        public IActionResult UploadFile( IFormFile file, [FromForm] string? customFileName)
        {
            return Ok(new UploadHandle().Upload(file,customFileName));
        }
        [HttpGet("view-file")]
        public IActionResult ViewFile(string? fileName)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(uploadPath))
                return NotFound("Uploads directory does not exist.");

            var files = Directory.GetFiles(uploadPath).Select(Path.GetFileName).ToList();

            // Nếu không nhập fileName, trả về danh sách file
            if (string.IsNullOrEmpty(fileName))
                return Ok(files);

            // Tìm file có chứa từ khóa tìm kiếm (không phân biệt hoa thường)
            var matchedFile = files.FirstOrDefault(f => f.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0);

            if (matchedFile == null)
                return NotFound("No matching file found.");

            var filePath = Path.Combine(uploadPath, matchedFile);

            // Lấy contentType chính xác
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string? contentType))
            {
                contentType = "application/octet-stream"; // Loại file không xác định
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
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
            return Ok(_animal.GetAnimalByType(type,20));
        }
    }
}
