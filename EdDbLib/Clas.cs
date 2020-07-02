using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib
{
    public class Clas
    {
        public int Id { get; set; }
        private string _code = string.Empty;
        public string Code
        { 
            get { return _code; } 
            set { 
                 if (value.Length > 6) 
                    { throw new Exception("Code length must be <= 6"); }
                _code = value;
                } 
        }
        private string _subject = string.Empty;
        public string Subject
        {
            get { return _subject; }
            set
            {
                if(value.Length > 30)
                {
                    throw new Exception("Subject length must be <= 30");
                }
                _subject = value;
            }
        }
        public int Section { get; set; }
        public int? InstructorId { get; set; } = null;


        public Clas(int id) { this.Id = id; }
        public Clas() : this(0) { } /* what is this even for again?*/

    }
}
