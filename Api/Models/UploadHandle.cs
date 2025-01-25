namespace Api.Models
{
    public class UploadHandle
    {
        public string Upload(IFormFile file)
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
                return "Maximum file size can be 5MB"+size;
            }

            // Lấy tên tệp gốc và loại bỏ các ký tự không hợp lệ
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string sanitizedFileName = string.Concat(originalFileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

            // Thêm thời gian để tránh trùng lặp tên
            string fileName = $"{sanitizedFileName}{extension}";

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // Tạo thư mục nếu chưa tồn tại
            }

            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            return fileName; // Trả về tên tệp đã lưu
        }
    }
}
