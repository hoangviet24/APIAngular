using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Repository
{
    public class AnimalRepository:IAnimalRepository
    {
        private readonly DataContext _context;
        public AnimalRepository(DataContext context) {
            _context = context;
        }

        public List<Animal> GetAnimalByName(string name)
        {
            var putId = _context.Animals.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            return putId.ToList();
        }

        AddAnimalDto IAnimalRepository.AddAnimal(AddAnimalDto animal)
        {
            var add = new Animal
            {
                Name = animal.Name,
                Description = animal.Description,
                Type = animal.Type,
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

        List<Animal> IAnimalRepository.GetAllAnimals()
        {
            return _context.Animals.ToList();
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
    }
}
