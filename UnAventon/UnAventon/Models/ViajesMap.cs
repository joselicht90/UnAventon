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
            Map(x => x.NViaje, "N_VIAJE").Not.Nullable();
            Map(x => x.Periodico, "PERIODICO").Not.Nullable();
            Map(x => x.CalificacionConductor, "CALIFICACION_CONDUCTOR").Not.Nullable();
            References(x => x.Destino).Column("DESTINO_ID").Not.Nullable();
            References(x => x.Origen).Column("ORIGEN_ID").Not.Nullable();
            References(x => x.Conductor).Column("CONDUCTOR_ID").Not.Nullable();
            References(x => x.Auto).Column("AUTO_ID").Not.Nullable();
            HasMany(x => x.Pasajeros).KeyColumn("VIAJES_ID").Cascade.AllDeleteOrphan();

        }
    }
}