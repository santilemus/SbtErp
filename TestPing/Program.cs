using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace TestPing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Ingresar Host: ");
            string sHost = Console.ReadLine();
            SqlString sqlHost = sHost;
            SqlInt16 timeOut = 10000;
            SqlInt32 result = SBT.Apps.Utils.PingHost.DoPing(sqlHost, timeOut);
            Console.WriteLine(string.Format("Resultado {0}", result.Value));
            Console.WriteLine("Presionar cualquier tecla para finalizar");
            Console.Read();
        }
    }
}
