using Api.Data;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        private readonly string cacheKey = "ShuffledAnimals";
        private readonly TimeSpan cacheDuration = TimeSpan.FromHours(24);
        public AnimalRepository(DataContext context,IMemoryCache cache) {
            _context = context;
            _cache = cache;
        }

        public List<Animal> GetAnimalByName(string name)
        {
            var getname = _context.Animals.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            return getname.ToList();
        }


        AddAnimalDto IAnimalRepository.AddAnimal(AddAnimalDto animal)
        {
            var add = new Animal()
            {
                Name = animal.Name,
                Type = animal.Type,
                Description = animal.Description,
                img = animal.img
            };
            _context.Animals.Add(add);
            _context.SaveChanges();
            return animal;
        }

        Animal IAnimalRepository.Delete(int id)
        {
            var putId = _context.Animals.FirstOrDefault(a => a.Id == id);
            if(putId != null)
            {
                _context.Animals.Remove(putId);
                _context.SaveChanges();
            }
            return putId;
        }

        public List<Animal> GetAllAnimals()
        {
            return _context.Animals.OrderBy(a => Guid.NewGuid()).ToList();
        }


        Animal IAnimalRepository.GetAnimalById(int id)
        {
            var putId = _context.Animals.Where(a => a.Id == id);
            return putId.FirstOrDefault();
        }

        AddAnimalDto IAnimalRepository.PutAnimal(int id, AddAnimalDto animal)
        {
            var putId = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (putId != null)
            {
                putId.Name = animal.Name;
                putId.Description = animal.Description;
                putId.Type = animal.Type;
                putId.img = animal.img;
                _context.SaveChanges();
            }
            return animal;
        }

        public List<Animal> GetAnimalByType(string type,int count = 20)
        {
            var getType = _context.Animals.Where(x=> x.Type == type);
            return getType.Take(count).ToList();
        }

        public List<Animal> GetAllRandomAnimals(int count = 10)
        {
            if (!_cache.TryGetValue(cacheKey, out List<Animal> animals))
            {
                animals = _context.Animals.OrderBy(a => Guid.NewGuid()).Take(count).ToList();
                _cache.Set(cacheKey, animals, cacheDuration);
            }
            return animals;
        }
    }
}
