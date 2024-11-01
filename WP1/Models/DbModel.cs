using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WP1.Models
{
    public enum Gender { Male=1, Female}
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(40)]
        public string Name { get; set; }
        [Required, EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        
        [Required, Column(TypeName ="Date")]
        public DateTime JoiningDate { get; set; }
        [Required, StringLength(50)]
        public string Picture { get; set; }
        public bool IsActive {  get; set; }
        //Navigation
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
    public class Attendance
    {
        public int AttendanceId { get; set; }
        [Required, Column(TypeName ="Date")]
        public DateTime AttendanceDate { get; set; }
        [Required]
        public TimeSpan InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        //Fk
        [Required, ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        //navigation
        public virtual Employee Employee { get; set; }
    }



    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Employee> Employees { get; set;}
        public DbSet<Attendance> Attendances { get; set; }
    }













}