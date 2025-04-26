using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentClasses = new HashSet<StudentClass>();
            StudentsCourses = new HashSet<StudentsCourse>();
        }

        public int StudentId { get; set; }
        public string StudentCode { get; set; } = null!;
        public string? FullName { get; set; }
        public int? MajorId { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Major? Major { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }
        public virtual ICollection<StudentsCourse> StudentsCourses { get; set; }
    }
}
