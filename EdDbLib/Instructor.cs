using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib
{
    public class Instructor
    {
        public int Id { get; set; } = 0;
        private string _firstName = string.Empty;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if(value.Length >30)
                {
                    throw new Exception("FirstName length must be <= 30");
                }
            }
        }
        private string _lastName = string.Empty;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if(value.Length >30)
                {
                    throw new Exception("LastName must be <= 30");
                }
            }
        }
        public int YearsExperience { get; set; } = 0;
        public bool IsTenured { get; set; } = false;

        public string Fullname { get => @"{FirstName} {LastName}"; }
        public Instructor(int id)
        {
            this.Id = id;
        }
        public Instructor() : this(0) { }
    }
}
