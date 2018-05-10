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
<<<<<<< HEAD
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Precio, "PRECIO").Not.Nullable();
            Map(x => x.FechaViaje, "FECHAVIAJE").Not.Nullable();
            Map(x => x.FechaBaja, "FECHA_BAJA").Not.Nullable();
=======
            Id(x => x.Id, "ID").GeneratedBy.Sequence("SEQ_VIAJES_ID");
>>>>>>> 7922461fc9887e2ed57d3f0eb2aa653d9f32062d
            Map(x => x.FechaAlta, "FECHA_ALTA").Not.Nullable();
            Map(x => x.NViaje, "NVIAJE").Not.Nullable();
            Map(x => x.Periodico, "PERIODICO").Not.Nullable();
            Map(x => x.CalificacionConductor, "CALIFICACIONCONDUCTOR").Not.Nullable();
            
        }
    }
}