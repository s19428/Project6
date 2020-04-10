using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lecture6.Exceptions;
using Lecture6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lecture6.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            list.Add(new Student
            {
                IdStudent = 1,
                FirstName = "Jan",
                LastName = "Kowalski"
            });
            list.Add(new Student
            {
                IdStudent = 2,
                FirstName = "Andrzej",
                LastName = "Malewicz"
            });

            throw new StudentCannotDefendException("Student cannot defend because....");

            return Ok(list);
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(string index)
        {
            return Ok(new Student { FirstName = "Andrzej", LastName = "Malewicz" });
        }

    }
}