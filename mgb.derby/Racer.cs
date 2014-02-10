using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mgb.derby
{
    class Racer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Racer(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }        
    }
}
