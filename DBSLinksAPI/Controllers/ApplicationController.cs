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
	[Route("api/v1/application")]
	public class ApplicationController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ApplicationController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL APPLICATIONS
		//GET: api/v1/application

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<Application>>> Get()
		{
			var model = await (from t in _db.Applications
							   select new Application
							   {
								   ApplicationId = t.ApplicationId,
								   ApplicationName = t.ApplicationName
							   })
							   .AsNoTracking()
							   .ToListAsync();
			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/application/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<Application>> Get(int id)
		{
			var model = await _db.Applications
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.ApplicationId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/application

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Application>> Insert([FromBody] Application model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (model.ApplicationName == null)
			{
				return BadRequest("Unable to update the Application. Required fields missing");
			}

			var application = await (from c in _db.Applications
							  where c.ApplicationName == model.ApplicationName
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (application != null)
			{
				return StatusCode(409, "Unable to create the Application. The ApplicationName already exists in teams");
			}

			await _db.Applications.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Application has been Created");
		}

		//UPDATE: api/v1/application/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<Application>> Update(int id, [FromBody] Application model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.ApplicationId)
			{
				return BadRequest();
			}

			if (model.ApplicationName == null)
			{
				return BadRequest("Unable to update the Application. Required field missing");
			}

			var application = await (from c in _db.Applications
							  where c.ApplicationId != model.ApplicationId
								&& c.ApplicationName == model.ApplicationName
							  select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (application != null)
			{
				return StatusCode(409, "Unable to update the Application. The ApplicationName already exists in teams");
			}

			_db.Applications.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/application/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var application = await _db.Applications
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.ApplicationId == id);

			if (application == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.Applications.Remove(application); 
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}
	}
}

