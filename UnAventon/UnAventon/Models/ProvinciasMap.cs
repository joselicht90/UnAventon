using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class ProvinciasMap : ClassMap<Provincias>
    {
        public ProvinciasMap()
        {
            Table("UA_PROVINCIAS");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Nombre, "NOMBRE").Not.Nullable();

        }
    }
}




