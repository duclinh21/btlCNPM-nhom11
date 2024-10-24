using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Course
    {
        public Course()
        {
            ClassCourses = new HashSet<ClassCourse>();
            StudentsCourses = new HashSet<StudentsCourse>();
        }

        public int CourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }

        public virtual ICollection<ClassCourse> ClassCourses { get; set; }
        public virtual ICollection<StudentsCourse> StudentsCourses { get; set; }
    }
}
