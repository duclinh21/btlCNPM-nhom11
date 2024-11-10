using ScoreManagement.Models;

namespace ScoreManagement.ViewModels
{
    public class StudentReportViewModel
    {
        public string? SemesterCode { get; set; }
        public string? CourseName { get; set; }

        public string? CourseCode { get; set; }
        public double? Assignment1 { get; set; }
        public double? Assignment2 { get; set; }
        public double? Assignment3 { get; set; }
        public double? ProgressTest1 { get; set; }
        public double? ProgressTest2 { get; set; }
        public double? ProgressTest3 { get; set; }
        public double? FinalExam { get; set; }
        public double? AverageScore { get; set; }
        public string? Status { get; set; }

       
    }
}
