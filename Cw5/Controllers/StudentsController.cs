using Cw5.DTOs.Requests;
using Cw5.Models_Cw10;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private s16560Context db;
        
        public StudentsController(s16560Context context)
        {
            db = context;
        }

        //zwraca listę studentów
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(db.Student.ToList());
        }

        //zwraca studenta po indeksie 
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {          
            var s = db.Student.Find(indexNumber);
            if (s == null)
                return NotFound("Student o indeksie " + indexNumber + " nie istnieje");
            else                         
                return Ok(s);           
        }

        [HttpPost("modifyStudentData")]
        public IActionResult ModifyStudentData(ModifyStudentDataRequest request) 
        {
            var s = db.Student.Find(request.IndexNumber);
            if (s == null)
                return BadRequest("Student o indeksie " + request.IndexNumber + " nie istnieje");
            else
            {
                s.FirstName = request.FirstName;
                s.LastName = request.LastName;
                s.BirthDate = request.BirthDate;
                s.IdEnrollment = request.IdEnrollment;

                db.SaveChanges();
                return Ok(s);
            }                                

        }

        [HttpDelete("deleteStudent/{indexNumber}")]
        public IActionResult deleteStudent(string indexNumber)
        {
            var s = db.Student.Find(indexNumber);
            if (s == null)
                return BadRequest("Student o indeksie " + indexNumber + " nie istnieje");
            else
            {
                db.Student.Remove(s);
                db.SaveChanges();

                return Ok("Usunięto studenta " + indexNumber);
            }
        }
    }

}
