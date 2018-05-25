using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UnAventon.Models;
using UnAventon.NHibernate;

namespace UnAventon.Controllers
{
    public class ViajeController : Controller
    {
        public ActionResult DetalleViaje(long idViaje)
        {
            try
            {
                ISession session = NHibernate.NHibernateHelper.GetCurrentSession();
                Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                return View(viaje);
            }
            catch (Exception)
            {
                return View();
            }

        }


        public ActionResult borrarViaje(long idViaje)
        {
            ISession session = NHibernate.NHibernateHelper.GetCurrentSession();
            Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
            session.Delete(viaje);
            session.Flush();
            return View();
        }

        public JsonResult GuardarViaje(string tipo, string origen, string destino, string auto, string costo, string duracion)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = new Viajes();
                    viaje.CalificacionConductor = 0;
                    viaje.Precio = decimal.Parse(costo);
                    viaje.FechaAlta = DateTime.Today;
                    viaje.FechaViaje = DateTime.Today.AddDays(1);
                    viaje.Periodico = long.Parse(tipo);
                    Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
                    viaje.Conductor = usuarioLogueado;
                    viaje.Auto = (Autos)session.Get(typeof(Autos), long.Parse(auto));
                    viaje.Origen = (Localidades)session.Get(typeof(Localidades), long.Parse(origen));
                    viaje.Destino = (Localidades)session.Get(typeof(Localidades), long.Parse(destino));
                    session.Save(viaje);
                    transaction.Commit();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar guardar el nuevo auto." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}