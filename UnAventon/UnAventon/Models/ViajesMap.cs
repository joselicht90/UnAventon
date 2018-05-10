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
            Map(x => x.Precio, "PRECIO").Not.Nullable();
            Map(x => x.FechaViaje, "FECHAVIAJE").Not.Nullable();
            Map(x => x.FechaBaja, "FECHA_BAJA").Not.Nullable();
            Map(x => x.FechaAlta, "FECHA_ALTA").Not.Nullable();
            Map(x => x.NViaje, "NVIAJE").Not.Nullable();
            Map(x => x.Periodico, "PERIODICO").Not.Nullable();
            Map(x => x.CalificacionConductor, "CALIFICACIONCONDUCTOR").Not.Nullable();
            
        }
    }
}