using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class StudentsCourse
    {
        public StudentsCourse()
        {
            Grades = new HashSet<Grade>();
        }

        public int StudentCourseId { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public int? ClassId { get; set; }
        public int? SemesterId { get; set; }
        public int? LecturerId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Lecturer? Lecturer { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Student? Student { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
