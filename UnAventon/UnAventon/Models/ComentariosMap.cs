using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class ComentariosMap : ClassMap<Comentarios>
    {
        public ComentariosMap()
        {
            Table("UA_COMENTARIOS");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Texto, "TEXTO").Not.Nullable();
        }
    }
}




