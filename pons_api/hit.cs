using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pons_api {
    class hit {
        public string type { get; set; }
        public bool opendict { get; set; }

        public List<rom> roms { get; set; }
    }
}
