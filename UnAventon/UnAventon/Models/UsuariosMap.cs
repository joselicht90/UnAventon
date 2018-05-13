using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class UsuariosMap : ClassMap<Usuarios>
    {
        public UsuariosMap()
        {
            Table("UA_USUARIOS");
            Id(x => x.Id, "ID").GeneratedBy.Native();
            Map(x => x.Nombre, "NOMBRE").Not.Nullable();
            Map(x => x.Apellido, "APELLIDO").Not.Nullable();
            Map(x => x.Email, "EMAIL").Not.Nullable();
            Map(x => x.FNacimiento, "F_NACIMIENTO").Not.Nullable();
            Map(x => x.CReputacion, "C_REPUTACION").Not.Nullable();
            Map(x => x.PReputacion, "P_REPUTACION").Not.Nullable();
            Map(x => x.Password, "PASSWORD").Not.Nullable();
        }
    }
}


