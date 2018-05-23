using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UnAventon.Models;

namespace UnAventon.Controllers
{
    public class ViajeController : Controller
    {
        public ActionResult NuevoViaje()
        {
            try
            {
                ISession session = NHibernate.NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    long idUsuario = ((Usuarios)Session["UsuarioLogueado"]).Id;
                    IList<Localidades> localidades = session.QueryOver<Localidades>().Where(x => x.Partido.Id == 9).List();
                    IList<Autos> autos = session.QueryOver<Autos>().Where(x => x.UsuarioId == idUsuario).List();
                    ViewBag.Localidades = localidades;
                    ViewBag.Autos = (autos != null) ? autos : new List<Autos>();
                    return View();
                }
            }
            catch(Exception)
            {
                return View();
            }
            
        }
    }
}