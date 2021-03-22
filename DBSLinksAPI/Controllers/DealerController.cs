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
	[Route("v1/dealer")]
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
		[Authorize]
		public async Task<ActionResult<List<Dealer>>> Get()
		{
			var model = await (from t in _db.Dealers
							   select new Dealer
							   {
								   DealerId = t.DealerId,
								   DealerName = t.DealerName
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

			if (model.DealerName == null)
			{
				return BadRequest("Unable to update the team. Required fields missing");
			}

			var dealer = await (from c in _db.Dealers
							  where c.DealerName == model.DealerName
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (dealer != null)
			{
				return StatusCode(409, "Unable to create the Dealer . The teamUserName or ComputerName already exists in teams");
			}

			await _db.Dealers.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The team has been Created");
		}

		//UPDATE: api/v1/dealer/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Dealer>> Update(int id, [FromBody] Team model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.TeamId)
			{
				return BadRequest();
			}

			if (model.TeamName == null)
			{
				return BadRequest("Unable to update the team. Required fields missing");
			}

			var team = await (from c in _db.Dealers
							  where c.TeamId != model.TeamId
								&& c.TeamName == model.TeamName
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (team != null)
			{
				return StatusCode(409, "Unable to update the team. The teamUserName or ComputerName already exists in teams");
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
			var team = await _db.Dealers
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.DealerId == id);

			if (team == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Dealers.Remove(team);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}
	}
}

