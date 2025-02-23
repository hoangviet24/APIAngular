using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; } =string.Empty;
        public string? Password { get; set; } =string.Empty;
        public bool? Role { get; set; } = false;
        public List<Favorite>? Favorite { get; set; }
    }
}
