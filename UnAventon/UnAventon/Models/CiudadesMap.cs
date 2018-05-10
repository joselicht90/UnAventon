using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class CiudadesMap : ClassMap<Ciudades>
    {
        public CiudadesMap()
        {
            Table("UA_CIUDADES");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Nombre, "NOMBRE").Not.Nullable();
            Map(x => x.Cp, "CP").Not.Nullable();
        }
           
    }
}





