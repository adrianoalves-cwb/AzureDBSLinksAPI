
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DBSLinksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBSLinksAPI.DBContext;
using System.Collections.Generic;

namespace DBSLinksAPI.Controllers
{
	[ApiController]
	[Route("api/books")]
	public class DefaultController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public DefaultController(ApplicationDbContext db)
		{
			_db = db;
		}

		/*
				//GET ALL

				[HttpGet]
				[Authorize]
				[Route("")]
				public async Task<ActionResult<List<Book>>> Get()
				{
					var model = await _db.Books
						.AsNoTracking()
						.ToListAsync();
					return model;
				}

				[HttpGet("{id}")]
				[Authorize]
				[Route("")]
				public async Task<ActionResult<Book>> Get(int? id)
				{
					var model = await _db.Books
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.Id == id);
					if (model != null)
					{
						return model;
					}
					return NotFound();
				}

				[HttpPost]
				[Route("Upsert")]
				public async Task<ActionResult<Book>> Upsert([FromBody] Book model)
				{
					string message;

					if (ModelState.IsValid)
					{
						if(model.Id == 0)
						{
							_db.Books.Add(model);
							message = "Insert successfull";
						}
						else
						{
							var modelFromDB = _db.Books
								.AsNoTracking()
								.FirstOrDefault(u => u.Id == model.Id);
							if (modelFromDB == null)
							{
								return NotFound();
							}
							_db.Books.Update(model);
							message = "Update successfull";
						}
						await _db.SaveChangesAsync();
						return Ok(message);
					}
					else
					{
						return BadRequest(ModelState);
					}
				}

				//DELETE

				[HttpDelete]
				[Route("Delete")]
				public async Task<IActionResult> Delete(int id)
				{
					var bookFromDb = await _db.Books
						.AsNoTracking()
						.FirstOrDefaultAsync(u => u.Id == id);
					if (bookFromDb == null)
					{
						return NotFound("Record not found");
					}
					_db.Books.Remove(bookFromDb);
					await _db.SaveChangesAsync();
					return Ok("Delete successfull");
				}

				//INSERT

				[HttpPost]
				[Route("/Books/Insert")]
				public IActionResult Insert([FromBody]Book model)
				{
					if (ModelState.IsValid)
					{
						_db.Books.Add(model);
						_db.SaveChanges();
						//return model;
						return Json(new { success = true, message = "Insert successfull" });
					}
					else
					{
						return BadRequest(ModelState);
					}
				}*/

		//[HttpPost]
		//[Route("Insert")]
		//public async Task<ActionResult<Book>> Insert([FromBody] Book model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _db.Books.Add(model);
		//        await _db.SaveChangesAsync();
		//        return Ok("Insert successfull");
		//    }
		//    else
		//    {
		//        return BadRequest(ModelState);
		//    }
		//}

		////UPDATE

		//[HttpPost]
		//[Route("Update")]
		//public async Task<ActionResult<Book>> Update([FromBody] Book model)
		//{
		//	if (ModelState.IsValid)
		//          {
		//		_db.Books.Update(model);
		//		await _db.SaveChangesAsync();
		//		return Ok("Update successfull"); ;
		//          }
		//          else
		//          {
		//		return BadRequest(ModelState);
		//          }
		//      }


	}
}