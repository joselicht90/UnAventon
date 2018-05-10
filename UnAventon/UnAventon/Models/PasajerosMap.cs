using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class PasajerosMap : ClassMap<Pasajeros>
    {
        public PasajerosMap()
        {
            Table("UA_PASAJEROS");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Estado, "ESTADO").Not.Nullable();
            Map(x => x.Calificacion, "CALIFICACION").Not.Nullable();

        }
    }
}

