using System;
using System.Net.NetworkInformation;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Security;

namespace SBT.Apps.Utils
{
    public class PingHost
    {
        [SqlFunction(DataAccess = DataAccessKind.None)]
        //[SecurityCritical()]
        [NetworkInformationPermission(System.Security.Permissions.SecurityAction.Demand, Access = "Full", Unrestricted = true)]
        public static  SqlString DoPing(SqlString host, SqlInt16 timeOut)
        {
            Ping ping = null;
            PingReply rp = null;
            try
            {
                using (ping = new Ping())
                {
                    rp = ping.Send(host.Value, timeOut.Value);
                    //exceptionMessage = "";
                    return Enum.GetName(typeof(System.Net.NetworkInformation.IPStatus), rp.Status);
                    //return Convert.ToInt32(rp.Status); // == IPStatus.Success;
                }
            }
            catch (PingException ex)
            {
                //resultado = Enum.GetName(typeof(System.Net.NetworkInformation.IPStatus), rp.Status);
                return ex.Message;
                //return -1;
            }
        }
    }
}
