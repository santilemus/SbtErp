using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Facturacion.Module
{
    public class DBColumn
    {
        public DBColumn()
        {

        }

        public DBColumn(int AIndex, IMemberInfo AMemberInfo)
        {
            Index = AIndex;
            MemberInfo = AMemberInfo;
        }

        public int Index { get; set; }
        public IMemberInfo MemberInfo { get; set; }
    }
}
