using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using WP1.Models;
using WP1.ViewModels;

namespace WP1.Controllers
{
    [Authorize]
    public class EmployeesController : ApiController
    {
        private readonly EmployeeDbContext db = new EmployeeDbContext();

        // GET: api/Employees
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }
        //With Child
        [Route("api/Employees/Attendance/Include")]
        public IQueryable<Employee> GetEmployeesWithChild()
        {
            return db.Employees.Include(x=>x.Attendances);
        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }
        //With Child Single
        [Route("api/Empoloyees/{id}/Include")]
        public IHttpActionResult GetEmployeeWithChild(int id)
        {
            Employee employee = db.Employees.Include(x=>x.Attendances).FirstOrDefault(x=>x.EmployeeId==id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

           var emp = db.Employees.FirstOrDefault(x=>x.EmployeeId==id);
            if (emp == null)
            {
                return NotFound();
            }
            emp.Name = employee.Name;
            emp.Gender = employee.Gender;
            emp.JoiningDate = employee.JoiningDate;
            emp.Picture = employee.Picture;
            emp.IsActive = employee.IsActive;
            db.Database.ExecuteSqlCommand($"Delete From Attendances Where EmployeeId={id}");
            foreach (var a in employee.Attendances)
            {
                db.Attendances.Add(new Attendance { EmployeeId=id, AttendanceDate=a.AttendanceDate, InTime=a.InTime, OutTime=a.OutTime});
            }


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
           
            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }
        //File Upload
        [HttpPost, Route("api/Employees/Image/Upload")]
        public IHttpActionResult Upload()
        {
            if(HttpContext.Current.Request.Files.Count > 0)
            {
                var file = HttpContext.Current.Request.Files[0];
                string ext = Path.GetExtension(file.FileName);
                string f =Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ ext;
                string savePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Pictures"), f);
                file.SaveAs(savePath);
                return Ok(new FileUploadResult { NewFileName = f});
            }
            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeId == id) > 0;
        }
    }
}