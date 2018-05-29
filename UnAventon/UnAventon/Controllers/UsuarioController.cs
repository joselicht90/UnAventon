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
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult MiPerfil()
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            Usuarios usuarioLogueado = session.QueryOver<Usuarios>().Where(x => x.Id == ((Usuarios)Session["UsuarioLogueado"]).Id).SingleOrDefault();
            return View(usuarioLogueado);
        }
        public JsonResult EditarAuto(string marca, string modelo, string patente, string asientos, string idAuto)
        {
            try
            {
                if(string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(asientos) || string.IsNullOrEmpty(idAuto))
                {
                    return Json(new { mensaje = "Todos los campos son obligatorios." }, JsonRequestBehavior.AllowGet);
                }
                ISession session = NHibernateHelper.GetCurrentSession();
                List<Viajes> viajesDelAuto = session.QueryOver<Viajes>().Where(x => x.Auto.Id == long.Parse(idAuto)).List().ToList();
                if(viajesDelAuto.Any(x=>x.FechaBaja != null))
                {
                    return Json(new { mensaje = "El auto se encuentra en un viaje activo y no puede ser modificado." }, JsonRequestBehavior.AllowGet);

                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Autos auto = session.Get<Autos>(long.Parse(idAuto));
                    auto.Asientos = long.Parse(asientos);
                    auto.Marca = marca;
                    auto.Modelo = modelo;
                    auto.Patente = patente;
                    session.Update(auto);
                    transaction.Commit();
                    long idUsuario = ((Usuarios)Session["UsuarioLogueado"]).Id;
                    IList<Autos> autos = session.QueryOver<Autos>().Where(x => x.UsuarioId == idUsuario).List();
                    Session["Autos"] = (autos != null) ? autos : new List<Autos>();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar guardar el nuevo auto." }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarAuto(string marca, string modelo, string patente, string asientos, string idUsuario)
        {
            try
            {
                if (string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(asientos))
                {
                    return Json(new { mensaje = "Todos los campos son obligatorios." }, JsonRequestBehavior.AllowGet);
                }
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Autos auto = new Autos();
                    auto.Asientos = long.Parse(asientos);
                    auto.Marca = marca;
                    auto.Modelo = modelo;
                    auto.Patente = patente;
                    Usuarios usuarioLogueado = session.QueryOver<Usuarios>().Where(x => x.Id == long.Parse(idUsuario)).SingleOrDefault();
                    auto.UsuarioId = usuarioLogueado.Id;
                    usuarioLogueado.Autos.Add(auto);
                    session.Update(usuarioLogueado);
                    transaction.Commit();
                    IList<Autos> autos = session.QueryOver<Autos>().Where(x => x.UsuarioId == long.Parse(idUsuario)).List();
                    Session["Autos"] = (autos != null) ? autos : new List<Autos>();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar guardar el nuevo auto." }, JsonRequestBehavior.AllowGet);
            }
        }

        
        public JsonResult BorrarAuto(string id)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Autos auto = session.QueryOver<Autos>().Where(x => x.Id == long.Parse(id)).SingleOrDefault();
                    session.Delete(auto);
                    transaction.Commit();
                    long idUsuario = ((Usuarios)Session["UsuarioLogueado"]).Id;
                    IList<Autos> autos = session.QueryOver<Autos>().Where(x => x.UsuarioId == idUsuario).List();
                    Session["Autos"] = (autos != null) ? autos : new List<Autos>();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar borrar el nuevo auto." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}