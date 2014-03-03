using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Semantika.EuropeanaInside.Eck
{
    public class Logger:TraceListener
    {
         private readonly bool _state;

        private readonly string _logname =  "Log.txt";

        public Logger(bool state, string logname)
        {
            _state = state;
            _logname = logname;
        }


        public override void Write(string message)
        {
            if (_state)
            {
                WriteLine(message);
            }
        }

        public override void WriteLine(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("/Logs/" + _logname);

                if (_state)
                {
                    string alltext = DateTime.Now + @"===>" + message;
                    File.AppendAllText(path, alltext + Environment.NewLine + Environment.NewLine);
                    Console.WriteLine(alltext);
                }
            }
            catch
            {
                
            }
        }
    }


}
