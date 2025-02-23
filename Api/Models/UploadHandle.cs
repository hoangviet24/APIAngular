using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Models
{
    public class UploadHandle
    {
        public string Upload(IFormFile file, string? customFileName)
        {
            List<string> validExtension = new List<string>() { ".jpg", ".png", ".jpeg" };
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (!validExtension.Contains(extension))
            {
                return $"Extension is not valid {string.Join(',', validExtension)}";
            }

            long size = file.Length;
            if (size > 5 * 1024 * 1024)
            {
                return $"Maximum file size is 5MB, your file is {size} bytes";
            }

            // Nếu có tên file tùy chỉnh, dùng nó. Nếu không, lấy tên file gốc
            string fileName = !string.IsNullOrWhiteSpace(customFileName)
                ? $"{customFileName}{extension}"
                : Path.GetFileNameWithoutExtension(file.FileName) + extension;

            // Loại bỏ ký tự không hợp lệ trong tên file
            fileName = string.Concat(fileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string finalFileName = fileName;
            int count = 1;
            while (File.Exists(Path.Combine(path, finalFileName)))
            {
                finalFileName = $"{Path.GetFileNameWithoutExtension(fileName)}({count}){extension}";
                count++;
            }

            using FileStream stream = new FileStream(Path.Combine(path, finalFileName), FileMode.Create);
            file.CopyTo(stream);

            return finalFileName;
        }
    }
}
