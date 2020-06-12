using System;
using System.Linq;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models_Cw10;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;


namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    
    public class EnrollmentsController : ControllerBase
    {
        private s16560Context db;

        public EnrollmentsController(s16560Context dbContext)
        {
            db = dbContext;
        }

        [HttpPost]       
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            //czy index jest unikalny           
            if (db.Student.Any(s => s.IndexNumber.Equals(request.IndexNumber)))           
                return BadRequest("Student " + request.IndexNumber + " już istnieje");

            //czy studia istnieją
            if(!db.Studies.Any(s => s.Name.Equals(request.Studies)))
                return BadRequest("Studia " + request.Studies + " nie istnieją");

            //czy istnieje wpis dla studiow, z semestrem 1

            var studies = db.Studies.Where(s => s.Name.Equals(request.Studies)).First();
            Enrollment enrollment = null;
            if (!db.Enrollment.Any(e => e.Semester == 1 && e.IdStudy == studies.IdStudy))
            {
                enrollment = new Enrollment();
                enrollment.IdEnrollment = db.Enrollment.Max(e => e.IdEnrollment) + 1;
                enrollment.Semester = 1;
                enrollment.IdStudy = studies.IdStudy;
                enrollment.StartDate = DateTime.Now;

                db.Enrollment.Add(enrollment);
            }
            else
            {
                enrollment = db.Enrollment.Where(e => e.Semester == 1 && e.IdStudy == studies.IdStudy).First();
            }

            //zapisanie studenta

            var student = new Student();
            student.IndexNumber = request.IndexNumber;
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;
            student.IdEnrollment = enrollment.IdEnrollment;

            var response = new EnrollStudentResponse();
            response.IndexNumber = request.IndexNumber;
            response.FirstName = request.FirstName;
            response.LastName = request.LastName;
            response.Studies = request.Studies;
            response.Semester = 1;

            db.Student.Add(student);
            db.SaveChanges();

            return Created("Zapisano studenta", response);
            
        }


        [HttpPost("{promotions}")]      
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            //czy studia istnieją
            var studies = db.Studies.Where(s => s.Name.Equals(request.Studies));
           
           if (studies.Count() == 0)
                return BadRequest("Studia " + request.Studies + " nie istnieją");

            var study = studies.First();

            //czy enrollment istnieje
            var enrollment = db.Enrollment.Where(e => e.Semester == request.Semester
                            && e.IdStudy == study.IdStudy);

            if (enrollment.Count() == 0)
                return NotFound("Wpis na semestr " + request.Semester + " dla " + request.Studies + " nie istnieje");
        
            //czy istnieje enrollment z semestrem o 1 wyższym
            var enrollmentPlus = db.Enrollment.Where(e => e.Semester == request.Semester + 1
                            && e.IdStudy == study.IdStudy);

            //jesli nie to dodajemy
            Enrollment resultEnrollmentPlus;
            if (enrollmentPlus.Count() == 0)
            {
                resultEnrollmentPlus = new Enrollment();
                resultEnrollmentPlus.IdEnrollment = db.Enrollment.Max(e => e.IdEnrollment) + 1;
                resultEnrollmentPlus.Semester = enrollment.First().Semester + 1;
                resultEnrollmentPlus.IdStudy = enrollment.First().IdStudy;
                resultEnrollmentPlus.StartDate = DateTime.Now;

                db.Enrollment.Add(resultEnrollmentPlus);
            }
            else 
            {
                resultEnrollmentPlus = enrollmentPlus.First();
            }

            //uaktualnienie dla studentów

            db.Student.Where(s => s.IdEnrollment == enrollment.First().IdEnrollment)
                            .ToList()
                            .ForEach(s => s.IdEnrollment = resultEnrollmentPlus.IdEnrollment);

            db.SaveChanges();
            return Ok("Promocja zakończona");

        }


    }
}


