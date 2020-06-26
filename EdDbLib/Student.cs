using System;

namespace EdDbLib
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Statecode { get; set; }
        public int SAT { get; set; }
        public decimal GPA { get; set; }
        public int? MajorId { get; set; }
        //question mark allows type to be null

        public Student(int id)
        {
            this.Id = id;
        }
        public Student() : this(0) { }
    }
}
