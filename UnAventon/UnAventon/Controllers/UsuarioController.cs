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
        public JsonResult GuardarAuto(string marca, string modelo, string patente, string asientos, string idUsuario)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Autos auto = new Autos();
                    auto.Asientos = long.Parse(asientos);
                    auto.Marca = marca;
                    auto.Modelo = modelo;
                    auto.Patente = patente;
                    Usuarios usuarioLogueado = session.QueryOver<Usuarios>().Where(x => x.Id == long.Parse(idUsuario)).SingleOrDefault();
                    usuarioLogueado.Autos.Add(auto);
                    session.Update(usuarioLogueado);
                    transaction.Commit();
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
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar borrar el nuevo auto." }, JsonRequestBehavior.AllowGet);
            }
        }
}