using System;
using System.Collections.Generic;
using System.Linq;
  

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Mvc;
using StocksApi.Models;
using StocksApi.Controllers;
using Microsoft.AspNetCore.Cors;

namespace StocksApi.Controllers
{
	[EnableCors("AddCORSPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		SchoolDbContext schoolDbContext = new SchoolDbContext();
		[HttpGet]
		public ActionResult GetStudents()
		{
			var query = schoolDbContext.StudentMarks.ToList();
			return Ok(query);
		}
	}
}
