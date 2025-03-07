using Api.Data;
using Api.Models;

namespace Api.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _dataContext;
        public ContactRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        ContactDto IContactRepository.Create(ContactDto contactDto)
        {
            var add = new Contact()
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                Title = contactDto.Title,
                Description = contactDto.Description,
            };
            _dataContext.Contacts.Add(add);
            _dataContext.SaveChanges();
            return contactDto;
        }

        Contact IContactRepository.Delete(int Id)
        {
            var getId = _dataContext.Contacts.Find(Id);
            if(getId == null)
            {
                return null;
            }
            _dataContext.Contacts.Remove(getId);
            _dataContext.SaveChanges() ;
            return getId;
        }

        List<Contact> IContactRepository.GetAll()
        {
            return _dataContext.Contacts.ToList();
        }
        List<Contact> IContactRepository.GetQuery(string query)
        {
            var getname = _dataContext.Contacts.Where(x=>x.Name.ToLower().Contains(query.ToLower()) || x.Title.ToLower().Contains(query.ToLower()));
            if (getname == null)
            {
                return null;
            }
            return getname.ToList();
        }
    }
}
