using Microsoft.AspNetCore.Mvc;
using StudentRosterApi.Models;

namespace StudentRosterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        // Simulated in-memory database
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, FirstName = "Juan", LastName = "Dela Cruz", Course = "BSCS", YearLevel = 3 },
            new Student { Id = 2, FirstName = "Maria", LastName = "Clara", Course = "BSIT", YearLevel = 2 }
        };

        // GET: api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(students); // Returns 200 OK
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found." }); // Returns 404
            }

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student newStudent)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(newStudent.FirstName) ||
                string.IsNullOrWhiteSpace(newStudent.LastName))
            {
                return BadRequest(new { message = "First name and last name are required." }); // Returns 400
            }

            // Auto-assign ID
            newStudent.Id = students.Any() ? students.Max(s => s.Id) + 1 : 1;

            students.Add(newStudent);

            // Returns 201 Created and includes the URI to the new resource
            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingStudent = students.FirstOrDefault(s => s.Id == id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            // Update properties
            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.Course = updatedStudent.Course;
            existingStudent.YearLevel = updatedStudent.YearLevel;

            return NoContent(); // Returns 204 No Content (Success but no body)
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);

            return NoContent();
        }
    }
}
