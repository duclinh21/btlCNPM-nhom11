using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public int? StudentCourseId { get; set; }
        public double? Assignment1 { get; set; }
        public double? Assignment2 { get; set; }
        public double? Assignment3 { get; set; }
        public double? ProgressTest1 { get; set; }
        public double? ProgressTest2 { get; set; }
        public double? ProgressTest3 { get; set; }
        public double? FinalExam { get; set; }
        public double? AverageScore { get; set; }
        public string? Status { get; set; }

        public virtual StudentsCourse? StudentCourse { get; set; }
    }
}
