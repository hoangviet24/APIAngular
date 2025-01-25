using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? img {  get; set; }
    }
}
