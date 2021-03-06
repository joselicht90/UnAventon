﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnAventon.Models
{
    public class Viajes
    {
        public Viajes() {
            Pasajeros = new List<Pasajeros>();
        }
        public virtual long Id { get; set; }
        public virtual decimal Precio { get; set; }
        public virtual DateTime FechaViaje { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual long NViaje { get; set; }
        public virtual long Periodico  { get; set; }
        public virtual long? CalificacionConductor { get; set; }
        public virtual Autos Auto { get; set; }
        public virtual Localidades Destino { get; set; }
        public virtual Localidades Origen { get; set; }
        public virtual Usuarios Conductor { get; set; }
        public virtual string Estado { get; set; }
        //public virtual int CantidadSemana { get; set; }
        public virtual int Duracion { get; set; }
        public virtual IList<Pasajeros> Pasajeros { get; set; }
        public virtual IList<Comentarios> Comentarios { get; set; }
    }
}