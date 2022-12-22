using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class FirebaseError
    {
        public Error error { get; set; }
    }
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Error> errors { get; set; }
    }
}
