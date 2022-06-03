using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCal.Interfaces
{
    public interface IInputPageVM
    {
        public DateTime BirthDate { get; set; }

        public DateTime StartDate { get; set; }

        public int Duration { get; set; }

    }
}
