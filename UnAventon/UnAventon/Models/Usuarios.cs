using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Usuarios
    {
        public Usuarios() { }
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime FNacimiento { get; set; }
        public virtual long CReputacion { get; set; }
        public virtual long PReputacion { get; set; }
        public virtual string Password { get; set; }
    }
}



