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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["UsuarioLogueado"] = null;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult LoginRegistro(string email, string password)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            ICriteria searchUsuario = session.CreateCriteria<Usuarios>();
            searchUsuario.Add(Expression.Eq("Email", email));
            searchUsuario.Add(Expression.Eq("Password", password));
            Usuarios usuario = searchUsuario.UniqueResult<Usuarios>();
            if (usuario != null)
            {
                Session["UsuarioLogueado"] = usuario;
                Session["UsuarioLogueadoNombre"] = usuario.Nombre;
            }
            return RedirectToAction("Index");
        }
        public JsonResult Login(string email, string password)
        {
            
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria searchUsuario = session.CreateCriteria<Usuarios>();
                    searchUsuario.Add(Expression.Eq("Email", email));
                    searchUsuario.Add(Expression.Eq("Password", password));
                    Usuarios usuario = searchUsuario.UniqueResult<Usuarios>();
                    if(usuario != null)
                    {
                        Session["UsuarioLogueado"] = usuario;
                        Session["UsuarioLogueadoNombre"] = usuario.Nombre;
                        return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { mensaje = "El nombre de usuario y/o contraseña son incorrectos" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar iniciar sesion." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RegistrarUsuario(string nombre, string apellido, string fechaNacimiento, string email ,string password, string confirmarPassword)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria searchUsuario = session.CreateCriteria<Usuarios>();
                    searchUsuario.Add(Expression.Eq("Email", email));
                    Usuarios usuarioRegistrado = searchUsuario.UniqueResult<Usuarios>();
                    if(usuarioRegistrado == null)
                    {
                        usuarioRegistrado = new Usuarios();
                        usuarioRegistrado.Nombre = nombre;
                        usuarioRegistrado.Apellido = apellido;
                        usuarioRegistrado.FNacimiento = DateTime.Parse(fechaNacimiento);
                        usuarioRegistrado.Password = password;
                        usuarioRegistrado.PReputacion = 0;
                        usuarioRegistrado.CReputacion = 0;
                        usuarioRegistrado.Email = email;
                        session.Save(usuarioRegistrado);
                        transaction.Commit();
                        Session["UsuarioLogueado"] = usuarioRegistrado;
                        return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { mensaje = "Ya existe un usuario registrado con este mail." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar registrar el usuario." }, JsonRequestBehavior.AllowGet);
            }
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