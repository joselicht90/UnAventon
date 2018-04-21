using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Viajes
    {
        public Viajes() { }
        public virtual long Id { get; set; }
        public virtual decimal Precio { get; set; }
        public virtual DateTime FechaViaje { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual long NViaje { get; set; }
        public virtual long Periodico  { get; set; }
        public virtual long CalificacionConductor { get; set; }
    }
}