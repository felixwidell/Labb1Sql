using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1SQL___.Models
{
    public class Elever
    {
        public int Id { get; set; }
        public string Firtsname { get; set; }
        public string Lastname { get; set; }

        public int ClassID { get; set; }
        public int GradeID { get; set; }
        public DateTime GradeDate { get; set; }
        
    }
}
