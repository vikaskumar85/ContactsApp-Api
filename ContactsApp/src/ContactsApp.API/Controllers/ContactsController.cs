using Microsoft.AspNetCore.Mvc;
using ContactsApp.Core.Interfaces;
using ContactsApp.Core.Models;

namespace ContactsApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IContactService _contactService;

        public ContactsController(ILogger<ContactsController> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResultModel>> GetAll()
        {
            var response = await _contactService.GetAll().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }

        [HttpPost]
        [Route("Insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Insert(ContactModel model)
        {
            if (model == null) { return BadRequest(); }

            var response = await _contactService.Insert(model);
            return response != null ? Ok(response) : NotFound();
        }

        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update(ContactModel model)
        {
            if (model == null) { return BadRequest(); }

            var response = await _contactService.Update(model);
            return response != null ? Ok(response) : NotFound();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            if (id == 0) { return BadRequest(); }

            var response = await _contactService.Delete(id);
            return response != null ? Ok(response) : NotFound();
        }
    }
}
