using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Autos
    {
        public Autos() { }
        public virtual long Id { get; set; }
        public virtual string Marca { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string Patente { get; set; }
        public virtual long Asientos { get; set; }
    }
}


