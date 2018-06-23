using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnAventon.Models;
using UnAventon.NHibernate;

namespace UnAventon.Controllers
{
    public class BusquedaController : Controller
    {
        // GET: Busqueda
        public ActionResult BuscarViaje()
        {

            return View();
        }
        public ActionResult BuscarViajes(string fechaDesde, string fechaHasta, long idOrigen, long idDestino)
        {
            DateTime fechaInicio = DateTime.Parse(fechaDesde);
            DateTime fechaFin = DateTime.Parse(fechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);
            ISession session = NHibernateHelper.GetCurrentSession();
            ICriteria criteria = session.CreateCriteria<Viajes>();
            criteria
                .CreateAlias("Origen","Origen")
                .CreateAlias("Destino","Destino")
                .Add(Expression.Ge("FechaViaje", fechaInicio))
                .Add(Expression.Le("FechaViaje", fechaFin))
                .Add(Expression.Eq("Origen.Id", idOrigen))
                .Add(Expression.Eq("Destino.Id", idDestino));
            List<Viajes> viajes = criteria.List<Viajes>().ToList();
            ViewBag.Viajes = viajes;
            return PartialView("_ListadoViajesBusqueda");
        }
    }
}