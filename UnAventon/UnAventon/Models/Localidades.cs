using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Localidades
    {
        public Localidades() { }
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual Partido Partido { get; set; }
    }
}





