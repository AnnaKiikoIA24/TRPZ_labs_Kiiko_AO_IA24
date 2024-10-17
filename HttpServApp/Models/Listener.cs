using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal class Listener
    {
        public int Port { get; } = Configuration.Port;
        public Listener() { }
        public Listener(int port)
        {
            Port = port;
        }
        public void RunServer() { }
    }
}
