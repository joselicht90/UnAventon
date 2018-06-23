using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;






namespace UnAventon.Models
{
    public class Comentarios
    {
        public Comentarios() { }
        public virtual long Id { get; set; }
        public virtual string Texto { get; set; }
        public virtual string Respuesta { get; set; }
        public virtual long ViajeId { get; set; }
    }
}



