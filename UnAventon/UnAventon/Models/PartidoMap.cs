using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class PartidoMap : ClassMap<Partido>
    {
        public PartidoMap()
        {
            Table("Departamento");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Nombre, "Nombre").Not.Nullable();

        }
    }
}




