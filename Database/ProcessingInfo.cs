using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWordGenerator.Database {
    class ProcessingInfo {
        public int Id { get; set; }
        public String NextPassword { get; set; }
        public long NoOfProcessedPasswords { get; set; }
    }
}
