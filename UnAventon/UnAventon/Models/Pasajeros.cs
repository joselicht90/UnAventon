using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Pasajeros
    {
        public Pasajeros() { }
        public virtual long Id { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Calificacion { get; set; }
    }
}



