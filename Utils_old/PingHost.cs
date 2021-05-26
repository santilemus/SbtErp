using System;
using System.Net.NetworkInformation;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;

namespace SBT.Apps.Utils
{
    public class PingHost
    {
        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static string DoPing(string host, out string exceptionMessage, int timeOut = 1000)
        {
            Ping ping = null;
            PingReply rp = null;
            try
            {
                ping = new Ping();
                rp = ping.Send(host, timeOut);
                exceptionMessage = "";
                return rp.Status.ToString();
            }
            catch (PingException ex)
            {
                exceptionMessage = ex.Message;
                return rp != null ? rp.ToString(): "";
            }
        }
    }
}
