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
	[Route("api/v1/applicationcategory")]
	public class ApplicationCategoryController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ApplicationCategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		//ALL APPLICATIONCATEGORIES
		//GET: api/v1/applicationcategory

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<ApplicationCategory>>> Get()
		{
			var model = await (from t in _db.ApplicationCategories
							   select new ApplicationCategory
							   {
								   ApplicationCategoryId = t.ApplicationCategoryId,
								   ApplicationCategoryName = t.ApplicationCategoryName
							   })
							   .AsNoTracking()
							   .ToListAsync();
			if (model != null)
			{
				return model;
			}
			return Ok("No Record could be found!");
		}

		//GET: api/v1/applicationcategory/5

		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<ApplicationCategory>> Get(int id)
		{
			var model = await _db.ApplicationCategories
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.ApplicationCategoryId == id);

			if (model != null)
			{
				return model;
			}
			return NotFound("The Record could not be found");
		}

		//INSERT: api/v1/applicationcategory

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<ApplicationCategory>> Insert([FromBody] ApplicationCategory model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (model.ApplicationCategoryName == null)
			{
				return BadRequest("Unable to update the Record. Required fields missing");
			}

			var applicationCategory = await (from c in _db.ApplicationCategories
									 where c.ApplicationCategoryName == model.ApplicationCategoryName
									 select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (applicationCategory != null)
			{
				return StatusCode(409, "Unable to create the Record. The Record already exists in the Database");
			}

			await _db.ApplicationCategories.AddAsync(model);
			await _db.SaveChangesAsync();

			return StatusCode(201, "The Record has been Created");
		}

		//UPDATE: api/v1/applicationcategory/5

		[HttpPut("{id:int}")]
		[Authorize]
		//[Route("")]
		public async Task<ActionResult<ApplicationCategory>> Update(int id, [FromBody] ApplicationCategory model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			if (id != model.ApplicationCategoryId)
			{
				return BadRequest();
			}

			if (model.ApplicationCategoryName == null)
			{
				return BadRequest("Unable to update the Record. Required field missing");
			}

			var applicationCategory = await (from c in _db.ApplicationCategories
									 where c.ApplicationCategoryId != model.ApplicationCategoryId
									   && c.ApplicationCategoryName == model.ApplicationCategoryName
									 select c)
								.AsNoTracking()
								.FirstOrDefaultAsync();

			if (applicationCategory != null)
			{
				return StatusCode(409, "Unable to update the Record. The Record already exists in teams");
			}

			_db.ApplicationCategories.Update(model);
			await _db.SaveChangesAsync();

			return Ok("Update successfull");
		}

		//DELETE: api/v1/applicationcategory/5

		[HttpDelete("{id:int}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.ApplicationCategories
							.AsNoTracking()
							.FirstOrDefaultAsync(u => u.ApplicationCategoryId == id);

			if (model == null)
			{
				return StatusCode(404, "The Record could not be found!");
			}

			_db.ApplicationCategories.Remove(model);
			await _db.SaveChangesAsync();

			return Ok("Delete successfull");
		}
	}
}

