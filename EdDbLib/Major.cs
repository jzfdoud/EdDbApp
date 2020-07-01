using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EdDbLib
{
    public class Major
    {
        public static string SelectAll = "Select * From Major;";


        public int Id { get; set; } = 0;

        private string _code = string.Empty;
        public string Code 
        { 
            get 
            {
                return _code;
            } 
            set 
            {
                if (value.Length > 4)
                {
                    throw new Exception("Code length must be <= 4");
                }
                _code = value;
            } 
        }
        public string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value.Length > 50)
                { throw new Exception("Description must be <= 50"); }
                _description = value;
            }
        }

        public int _minSat = 400;
        public int MinSAT 
        {   get => _minSat;
            set
            {
                if(value < 400 || value > 1600)
                {
                    throw new Exception("MinSAT must be between 400 and 1600");
                }
                _minSat = value;
            }
        }


        public Major(int id) { this.Id = id; }




    }
}
