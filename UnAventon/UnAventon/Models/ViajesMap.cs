using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class ViajesMap: ClassMap<Viajes>
    {
        public ViajesMap()
        {
            Table("UA_VIAJES");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.FechaAlta, "FECHA_ALTA").Not.Nullable();
        }
    }
}