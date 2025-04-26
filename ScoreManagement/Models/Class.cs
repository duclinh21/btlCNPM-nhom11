using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassCourses = new HashSet<ClassCourse>();
            StudentClasses = new HashSet<StudentClass>();
            StudentsCourses = new HashSet<StudentsCourse>();
        }

        public int ClassId { get; set; }
        public string? ClassCode { get; set; }
        public int? SemesterId { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual ICollection<ClassCourse> ClassCourses { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }
        public virtual ICollection<StudentsCourse> StudentsCourses { get; set; }
    }
}
