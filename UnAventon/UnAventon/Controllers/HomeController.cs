using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnAventon.Models;
using UnAventon.NHibernate;

namespace UnAventon.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult GuardarViaje()
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            try
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = new Viajes();
                    viaje.FechaAlta = DateTime.Now.Date;
                    viaje.FechaViaje = DateTime.Now.AddDays(1);
                    viaje.Periodico = 1;
                    viaje.Precio = decimal.Parse("1,25");
                    viaje.CalificacionConductor = 4;
                    session.Save(viaje);
                    transaction.Commit();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return null;
        }
    }
}