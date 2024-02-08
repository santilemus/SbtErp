using DevExpress.Xpo;
using System;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    public static class TestStoredProcHelper
    {

        public static DevExpress.Xpo.DB.SelectedData SelectfnPlaEmpleadoPlanilla(Session session, int OidEmpresa, int OidEmpleado, DateTime FechaInicio, DateTime FechaFin, DateTime FechaPago)
        {
            return session.ExecuteQuery("select * from fnPlaEmpleadoPlanilla(@OidEmpresa, @OidEmpleado, @FechaInicio, @FechaFin, @FechaPago)",
                new object[] { OidEmpresa, OidEmpleado, FechaInicio, FechaFin, FechaPago });
        }
        public static System.Collections.Generic.ICollection<TestEmpleadoPlanilla> fnPlaEmpleadoPlanillaIntoObjects(Session session, int OidEmpresa, int OidEmpleado, DateTime FechaInicio, DateTime FechaFin, DateTime FechaPago)
        {
            return session.GetObjectsFromQuery<TestEmpleadoPlanilla>("select * from fnPlaEmpleadoPlanilla(@OidEmpresa, @OidEmpleado, @FechaInicio, @FechaFin, @FechaPago)",
                new object[] { OidEmpresa, OidEmpleado, FechaInicio, FechaFin, FechaPago });
        }
        public static XPDataView fnPlaEmpleadoPlanillaIntoDataView(Session session, int OidEmpresa, int OidEmpleado, DateTime FechaInicio, DateTime FechaFin, DateTime FechaPago)
        {
            DevExpress.Xpo.DB.SelectedData selectData = session.ExecuteQuery("select * from fnPlaEmpleadoPlanilla(@OidEmpresa, @OidEmpleado, @FechaInicio, @FechaFin, @FechaPago)",
                new object[] { OidEmpresa, OidEmpleado, FechaInicio, FechaFin, FechaPago });
            return new XPDataView(session.Dictionary, session.GetClassInfo(typeof(TestEmpleadoPlanilla)), selectData);
        }
        public static void fnPlaEmpleadoPlanillaIntoDataView(XPDataView dataView, Session session, int OidEmpresa, int OidEmpleado, DateTime FechaInicio, DateTime FechaFin, DateTime FechaPago)
        {
            DevExpress.Xpo.DB.SelectedData selectData = session.ExecuteQuery("select * from fnPlaEmpleadoPlanilla(@OidEmpresa, @OidEmpleado, @FechaInicio, @FechaFin, @FechaPago)",
                new object[] { OidEmpresa, OidEmpleado, FechaInicio, FechaFin, FechaPago });
            dataView.PopulateProperties(session.GetClassInfo(typeof(TestEmpleadoPlanilla)));
            dataView.LoadData(selectData);
        }
    }
}
