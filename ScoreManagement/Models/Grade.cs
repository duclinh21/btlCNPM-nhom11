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
        public void CalculateAverageAndStatus()
        {
            // Tính toán AverageScore
            this.AverageScore = (((((Assignment1 * 0.1) + (Assignment2 * 0.1) + (Assignment3 * 0.1))
                                    + (ProgressTest1 * 0.1) + (ProgressTest2 * 0.1) + (ProgressTest3 * 0.1))
                                    + (FinalExam * 0.4)));

            // Tính toán Status dựa trên AverageScore
            this.Status = this.AverageScore > 5 ? "Pass" : "Not Pass";
        }
        public virtual StudentsCourse? StudentCourse { get; set; }
    }
}
