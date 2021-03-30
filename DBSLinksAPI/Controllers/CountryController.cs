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
	[Route("api/v1/country")]
	public class CountryController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public CountryController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL APPLICATIONCATEGORIES
		//GET: api/v1/country

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<Country>>> Get()
		{
			var model = await (from t in _db.Countries
							   select new Country
							   {
								   CountryId = t.CountryId,
								   CountryCode = t.CountryCode,
								   CountryName = t.CountryName
							   })
							   .AsNoTracking()
							   .ToListAsync();
			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/country/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<Country>> Get(int id)
		{
			var model = await _db.Countries
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.CountryId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/country

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Country>> Insert([FromBody] Country model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to update the Record. Required fields missing");
			}

			var country = await (from c in _db.Countries
											 where c.CountryCode == model.CountryCode
											 || c.CountryName == model.CountryName
											 select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (country != null)
			{
				return StatusCode(409, "Unable to create the Record. The Record already exists in the Database");
			}

			await _db.Countries.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Record has been Created");
		}

		//UPDATE: api/v1/country/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Country>> Update(int id, [FromBody] Country model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.CountryId)
			{
				return BadRequest();
			}

			if (ValidateRequiredFields(model) == false)
			{
				return BadRequest("Unable to update the Record. Required field missing");
			}

			var country = await (from c in _db.Countries
									where c.CountryCode == model.CountryCode
									|| c.CountryName == model.CountryName
									select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (country != null)
			{
				return StatusCode(409, "Unable to update the Record. The Record already exists in teams");
			}

			_db.Countries.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/country/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.Countries
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.CountryId == id);

			if (model == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Countries.Remove(model);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}

		private bool ValidateRequiredFields(Country model)
		{
			if (model.CountryId <= 0 || model.CountryCode <=0  || model.CountryName == null)
			{
				return false;
			}

			return true;
		}
	}
}

