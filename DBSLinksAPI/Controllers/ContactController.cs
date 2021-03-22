using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DBSLinksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DBSLinksAPI.Services;
using DBSLinksAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DBSLinksAPI.Controllers
{
	[ApiController]
	[Route("v1/contact")]
	public class ContactController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ContactController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL CONTACTS
		//GET: api/v1/contact

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<Contact>>> Get()
		{
			var model = await (from c in _db.Contacts
							   join t in _db.Teams
							   on c.TeamId equals t.TeamId
							   select new Contact
							   {
								   ContactId = c.ContactId,
								   ContactUserId = c.ContactUserId,
								   ContactName = c.ContactName,
								   ComputerName = c.ComputerName,
								   PhoneNumber = c.PhoneNumber,
								   CellPhone = c.CellPhone,
								   TeamId = c.TeamId,
								   TeamName = t.TeamName
							   })
							   .AsNoTracking()
							   .ToListAsync();

			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/contact/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<Contact>> Get(int id)
		{
			var model = await (from c in _db.Contacts
							   join t in _db.Teams
							   on c.TeamId equals t.TeamId
							   where c.ContactId == id
							   select new Contact
							   {
								   ContactId = c.ContactId,
								   ContactUserId = c.ContactUserId,
								   ContactName = c.ContactName,
								   ComputerName = c.ComputerName,
								   PhoneNumber = c.PhoneNumber,
								   CellPhone = c.CellPhone,
								   TeamId = c.TeamId,
								   TeamName = t.TeamName
							   })
							   .AsNoTracking()
							   .FirstOrDefaultAsync();

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/contact

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Contact>> Insert([FromBody] Contact model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to create the Contact. Required fields missing");
			}

			var contact = await (from c in _db.Contacts
								where c.ContactUserId == model.ContactUserId || c.ComputerName == model.ComputerName
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (contact != null)
			{
				return StatusCode(409, "Unable to create the Contact. The ContactUserName or ComputerName already exists in Contacts");
			}

			await _db.Contacts.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Contact has been Created");
		}

		//UPDATE: api/v1/contact/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Contact>> Update(int id, [FromBody] Contact model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.ContactId)
            {
				return BadRequest();
            }

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to update the Contact. Required fields missing");
			}

			var contact = await (from c in _db.Contacts
								where c.ContactId != model.ContactId
								&& (c.ContactUserId == model.ContactUserId || c.ComputerName == model.ComputerName)
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (contact != null)
			{
				return StatusCode(409, "Unable to update the Contact. The ContactUserName or ComputerName already exists in Contacts");
			}

			_db.Contacts.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/contact/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var contact = await _db.Contacts
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.ContactId == id);

			if (contact == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Contacts.Remove(contact);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}

		private bool ValidateRequiredFields(Contact contact)
        {
			if (contact.ContactUserId == null || contact.ContactName == null || contact.ComputerName == null || contact.TeamId <= 0)
            {
				return false;
			}

			var team = _db.Teams
						.AsNoTracking()
						.FirstOrDefault(u => u.TeamId == contact.TeamId);

			if (team == null)
            {
				return false;
            }
			return true;
        }
	} 
}

