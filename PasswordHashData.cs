using System;
using System.Collections.Generic;
using System.Text;

namespace TestWordGenerator
{
    public class PasswordHashData
    {
        public String Password{ get; set; }
        public String HashSha256 { get; set; }
        public String HashSha384 { get; set; }
        public String HashSha512 { get; set; }
    }
}
