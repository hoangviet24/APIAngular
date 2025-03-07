using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contact;
        public ContactController(IContactRepository contact)
        {
            _contact = contact;
        }
        [HttpPost]
        public IActionResult Create(ContactDto dto)
        {
            return Ok(_contact.Create(dto));
        }
        [HttpGet]
        public IActionResult Get([FromQuery] string? query) {
            if(query!= null)
            {

                return Ok(_contact.GetQuery(query));
            }
            else
            {
                return Ok(_contact.GetAll());
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id) {
            return Ok(_contact.Delete(id));
        }
    }
}
