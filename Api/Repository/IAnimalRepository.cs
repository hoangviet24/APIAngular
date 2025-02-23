using Api.Models;

namespace Api.Repository
{
    public interface IAnimalRepository
    {
        List<Animal> GetAllAnimals();
        List<Animal> GetAllRandomAnimals(int count);
        Animal GetAnimalById(int id);
        List<Animal>GetAnimalByName(string name);
        AddAnimalDto AddAnimal(AddAnimalDto animal);
        AddAnimalDto PutAnimal(int id, AddAnimalDto animal);
        List<Animal> GetAnimalByType(string type,int count);  
        Animal Delete(int id);
    }
}
