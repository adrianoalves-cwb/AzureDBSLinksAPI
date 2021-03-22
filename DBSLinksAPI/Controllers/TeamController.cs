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
	[Route("v1/team")]
	public class TeamController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public TeamController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL TEAMS
		//GET: api/v1/team

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<Team>>> Get()
		{
			var model = await (from t in _db.Teams
							   select new Team
							   {
								   TeamId = t.TeamId,
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

		//GET: api/v1/team/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<Team>> Get(int id)
		{
			var model = await _db.Teams
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.TeamId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/team

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Team>> Insert([FromBody] Team model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (model.TeamName == null)
			{
				return BadRequest("Unable to update the team. Required fields missing");
			}

			var team = await (from c in _db.Teams
							  where c.TeamName == model.TeamName
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (team != null)
			{
				return StatusCode(409, "Unable to create the team. The teamUserName or ComputerName already exists in teams");
			}

			await _db.Teams.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The team has been Created");
		}

		//UPDATE: api/v1/team/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Team>> Update(int id, [FromBody] Team model)
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

			var team = await (from c in _db.Teams
							  where c.TeamId != model.TeamId
								&& c.TeamName == model.TeamName
								select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (team != null)
			{
				return StatusCode(409, "Unable to update the team. The teamUserName or ComputerName already exists in teams");
			}

			_db.Teams.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/team/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var team = await _db.Teams
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.TeamId == id);

			if (team == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Teams.Remove(team);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}
	} 
}

