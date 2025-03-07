using Api.Models;
namespace Api.Repository
{
    public interface IContactRepository
    {
        List<Contact> GetAll();
        List<Contact >GetQuery(string query);
        ContactDto Create(ContactDto contactDto);
        Contact Delete(int Id);    
    }
}
