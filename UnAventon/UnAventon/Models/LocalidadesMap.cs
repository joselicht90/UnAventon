using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class LocalidadesMap : ClassMap<Localidades>
    {
        public LocalidadesMap()
        {
            Table("Localidad");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Nombre, "Nombre").Not.Nullable();
            References(x => x.Partido).Column("idDepartamento").Cascade.None();
        }
           
    }
}





