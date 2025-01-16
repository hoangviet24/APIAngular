using Api.Models;

namespace Api.Repository
{
    public interface IAnimalRepository
    {
        List<Animal > GetAllAnimals();
        Animal GetAnimalById(int id);
        List< Animal >GetAnimalByName(string name);
        AddAnimalDto AddAnimal(AddAnimalDto animal);
        AddAnimalDto PutAnimal(int id, AddAnimalDto animal);
        Animal Delete(int id);
    }
}
