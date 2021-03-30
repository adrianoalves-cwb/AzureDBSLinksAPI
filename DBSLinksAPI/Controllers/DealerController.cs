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
	[Route("api/v1/dealer")]
	public class DealerController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public DealerController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL DEALERS
		//GET: api/v1/dealer

		[HttpGet]
		//[Authorize]
		public async Task<ActionResult<List<Dealer>>> Get()
		{
			var model = await (from t in _db.Dealers
							   select new Dealer
							   {
								   DealerId = t.DealerId,
								   MainDealerId = t.MainDealerId,
								   CountryCode = t.CountryCode,
								   CTDI = t.CTDI,
								   DealerName = t.DealerName,
								   Branch = t.Branch,
								   PhoneNumber = t.PhoneNumber,
								   BaldoPartner = t.BaldoPartner,
								   IsActive = t.IsActive
							   })
							   .AsNoTracking()
							   .ToListAsync();
			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/dealer/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<Dealer>> Get(int id)
		{
			var model = await _db.Dealers
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.DealerId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/dealer

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Dealer>> Insert([FromBody] Dealer model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to create the Dealer. Required fields missing");
			}

			var dealer = await (from c in _db.Dealers
							  where c.DealerName == model.DealerName || c.CTDI == model.CTDI
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (dealer != null)
			{
				return StatusCode(409, "Unable to create the Dealer . The DealerName or CTDI already exists in Dealers");
			}

			await _db.Dealers.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Dealer has been Created");
		}

		//UPDATE: api/v1/dealer/5

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

			var dealer = await (from c in _db.Dealers
							  where c.DealerId != model.DealerId
								&& c.DealerName == model.DealerName
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (dealer != null)
			{
				return StatusCode(409, "Unable to update the Dealer. The record already exists in Dealers");
			}

			_db.Dealers.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/dealer/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.Dealers
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.DealerId == id);

			if (model == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Dealers.Remove(model);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}

		private bool ValidateRequiredFields(Dealer dealer)
		{
			if (dealer.DealerId <= 0 || dealer.DealerName == null || dealer.MainDealerId <=0 || dealer.CountryCode <= 0 || dealer.CTDI <= 0)
			{
				return false;
			}

			var country = _db.Countries
						.AsNoTracking()
						.FirstOrDefault(u => u.CountryCode == dealer.CountryCode);

			if (country == null)
			{
				return false;
			}
			return true;
		}
	}
}

