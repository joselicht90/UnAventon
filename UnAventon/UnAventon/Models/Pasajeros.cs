using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Pasajeros
    {
        public Pasajeros() {
            Calificacion = 0;
        }
        public virtual long Id { get; set; }
        public virtual string Estado { get; set; }
        public virtual long Calificacion { get; set; }
        public virtual long Viaje { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}



