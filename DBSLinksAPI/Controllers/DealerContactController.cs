﻿using System.Linq;
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
	[Route("v1/dealercontact")]
	public class DealerContactController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public DealerContactController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL DEALER CONTACTS
		//GET: api/v1/dealercontact

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<DealerContact>>> Get()
		{
			var model = await (from t in _db.DealerContacts
							   select new DealerContact
							   {
								   DealerContactId = t.DealerContactId,
								   MainDealerId = t.MainDealerId,
								   ContactName = t.ContactName,
								   PhoneNumber = t.PhoneNumber,
								   CellPhone = t.CellPhone,
								   Email = t.Email,
								   JobRole = t.JobRole,
								   Department = t.Department
								})
							   .AsNoTracking()
							   .ToListAsync();
			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/dealercontact/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<DealerContact>> Get(int id)
		{
			var model = await _db.DealerContacts
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.DealerContactId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/dealercontact

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<DealerContact>> Insert([FromBody] DealerContact model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to create the Dealer. Required fields missing");
			}

			var dealer = await (from c in _db.DealerContacts
								where c.DealerName == model.DealerName || c.CTDI == model.CTDI
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (dealer != null)
			{
				return StatusCode(409, "Unable to create the Dealer . The DealerName or CTDI already exists in Dealers");
			}

			await _db.DealerContacts.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Dealer has been Created");
		}

		//UPDATE: api/v1/dealercontact/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Dealer>> Update(int id, [FromBody] Dealer model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.DealerId)
			{
				return BadRequest();
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to create the Dealer. Required fields missing");
			}

			var dealer = await (from c in _db.DealerContacts
								where c.DealerId != model.DealerId
								  && c.DealerName == model.DealerName
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (dealer != null)
			{
				return StatusCode(409, "Unable to update the Dealer. The record already exists in Dealers");
			}

			_db.DealerContacts.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/dealercontact/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var team = await _db.DealerContacts
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.DealerId == id);

			if (team == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.DealerContacts.Remove(team);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}

		private bool ValidateRequiredFields(DealerContact dealerContact)
		{
			if (dealerContact.DealerContactId <= 0 || dealerContact.MainDealerId <=0 || dealerContact.ContactName == null)
			{
				return false;
			}

			var mainDealerId = _db.Dealers
						.AsNoTracking()
						.FirstOrDefault(u => u.MainDealerId == dealerContact.MainDealerId);

			if (mainDealerId == null)
			{
				return false;
			}
			return true;
		}
	}
}
