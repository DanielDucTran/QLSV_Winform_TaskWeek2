using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public class Course
    {
        public string   MHDK { get; set; }



        public Course(string MHDK)
        {
            this.MHDK = MHDK;
        }
            public override string ToString()
            {
            return MHDK;
            }
    }
}
