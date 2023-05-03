using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pons_api {
    public class rom {
        public string headword { get; set; }
        public string headword_full { get; set; }

        public string wordclass { get; set; }

        public List<arab> arabs { get; set; }
    }
}