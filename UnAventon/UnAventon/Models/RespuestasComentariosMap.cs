using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class RespuestasComentariosMap : ClassMap<RespuestasComentarios>
    {
        public RespuestasComentariosMap()
        {
            Table("UA_RESPUESTASCOMENTARIOS");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Texto, "TEXTO").Not.Nullable();
        }
    }
}


