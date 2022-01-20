using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEdge.Mail.Receiver
{
    public class Configuration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        //public bool DeleteMessages { get; set; }//DA TOGLIERE
    }
}
