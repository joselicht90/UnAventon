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
                long idUsuarioLogueado = ((Usuarios)Session["UsuarioLogueado"]).Id;
                ViewBag.IdUsuarioLogueado = idUsuarioLogueado;
                ISession session = NHibernate.NHibernateHelper.GetCurrentSession();
                Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                DatosDetalle(idUsuarioLogueado, viaje);
                return View(viaje);
            }
            catch (Exception)
            {
                return View();
            }

        }

        private void DatosDetalle(long idUsuarioLogueado, Viajes viaje)
        {
            if (viaje.Conductor.Id == idUsuarioLogueado)
            {
                ViewBag.ConductorDelViaje = true;
            }
            else
            {
                if (viaje.Pasajeros.Count > 0 && viaje.Pasajeros.Any(x => x.Usuario.Id == idUsuarioLogueado))
                {

                    Pasajeros pasajero = viaje.Pasajeros.Where(x => x.Usuario.Id == idUsuarioLogueado).First();
                    if (pasajero.Estado == "Rechazado")
                    {
                        ViewBag.Pagar = false;
                    }
                    if (pasajero.Estado == "Aceptado")
                    {
                        ViewBag.Pagar = true;
                    }

                }
                else
                {
                    ViewBag.Postular = true;
                }
            }
        }

        public JsonResult Postularse(long idViaje)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    int cantidadPasajeros = viaje.Pasajeros.Where(x => x.Estado.Equals("Aceptado")).Count();
                    if(cantidadPasajeros == viaje.Auto.Asientos)
                    {
                        return Json(new { mensaje = "Este viaje ya tiene el maximo de pasajeros." }, JsonRequestBehavior.AllowGet);
                    }
                    Pasajeros pasajero = new Pasajeros();
                    pasajero.Usuario = usuarioLogueado;
                    pasajero.Estado = "Postulado";
                    pasajero.Viaje = viaje.Id;
                    viaje.Pasajeros.Add(pasajero);
                    session.Update(viaje);
                    transaction.Commit();
                    DetalleViaje(viaje.Id);
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar postularse al viaje." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AceptarPasajero(long idViaje, long idPasajero)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    int cantidadPasajeros = viaje.Pasajeros.Where(x => x.Estado.Equals("Aceptado")).Count();
                    if (cantidadPasajeros == viaje.Auto.Asientos)
                    {
                        return Json(new { mensaje = "Este viaje ya tiene el maximo de pasajeros." }, JsonRequestBehavior.AllowGet);
                    }
                    viaje.Pasajeros.Where(x => x.Id == idPasajero).First().Estado = "Aceptado";
                    
                    session.Update(viaje);
                    transaction.Commit();
                    DetalleViaje(viaje.Id);
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar postularse al viaje." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RechazarPasajero(long idViaje, long idPasajero)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    Pasajeros pasajero = viaje.Pasajeros.Where(x => x.Id == idPasajero).First();
                    viaje.Pasajeros.Remove(pasajero);
                    session.Update(viaje);
                    transaction.Commit();
                    DetalleViaje(viaje.Id);
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar aceptar el pasajero." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RetirarPostulacion(long idViaje, long idPasajero)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    Pasajeros pasajero = viaje.Pasajeros.Where(x => x.Id == idPasajero).First();
                    if(pasajero.Estado.Equals("Aceptado") && pasajero.Usuario.CReputacion > 0)
                    {
                        pasajero.Usuario.CReputacion -= 1;
                    }
                    viaje.Pasajeros.Remove(pasajero);
                    //session.Delete(pasajero);
                    session.Update(viaje);
                    transaction.Commit();
                    DetalleViaje(viaje.Id);
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar aceptar el pasajero." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EliminarViaje(long idViaje)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    if (viaje.Pasajeros.Any(x=>x.Estado.Equals("Aceptado")) && viaje.Conductor.PReputacion>0)
                    {
                        viaje.Conductor.PReputacion -= 1;
                    }
                    session.Delete(viaje);
                    transaction.Commit();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar aceptar el pasajero." }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool ViajeValido(Usuarios usuarioLogueado, string fechaViaje, string duracion)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            IList<Viajes> viajesUsuario = session.QueryOver<Viajes>().Where(x => x.Conductor.Id == usuarioLogueado.Id).List();
            DateTime fechaFinViaje = DateTime.Parse(fechaViaje).AddMinutes(double.Parse(duracion));
            if (viajesUsuario.Any(x => x.FechaViaje <= DateTime.Parse(fechaViaje) && DateTime.Parse(fechaViaje) <= x.FechaViaje.AddMinutes((double)x.Duracion)))
            {
                return false;//return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
            }
            if (viajesUsuario.Any(x => x.FechaViaje <= DateTime.Parse(fechaViaje).AddMinutes(double.Parse(duracion)) && DateTime.Parse(fechaViaje).AddMinutes(double.Parse(duracion)) <= x.FechaViaje.AddMinutes((double)x.Duracion)))
            {
                return false;//return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
            }
            return true;
        }

        public JsonResult GuardarViaje(string tipo, string origen, string destino, string auto, string costo, string duracion, string fechaViaje, string cantidadSemanas)
        {
            Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (tipo.Equals("0"))
                    {

                        Viajes viaje = new Viajes();
                        viaje.CalificacionConductor = 0;
                        viaje.Precio = decimal.Parse(costo);
                        viaje.FechaAlta = DateTime.Today;
                        viaje.FechaViaje = DateTime.Parse(fechaViaje);
                        if (!ViajeValido(usuarioLogueado, viaje.FechaViaje.ToString(), duracion))
                        {
                            return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
                        }
                        viaje.Periodico = long.Parse(tipo);
                        viaje.Duracion = int.Parse(duracion);
                        viaje.Conductor = usuarioLogueado;
                        viaje.Auto = (Autos)session.Get(typeof(Autos), long.Parse(auto));
                        viaje.Origen = (Localidades)session.Get(typeof(Localidades), long.Parse(origen));
                        viaje.Destino = (Localidades)session.Get(typeof(Localidades), long.Parse(destino));
                        session.Save(viaje);
                        
                    }
                    else if (tipo.Equals("1"))
                    {
                        int dias = 7 * int.Parse(cantidadSemanas);
                        for(int i = 0; i < dias; i++)
                        {
                            Viajes viaje = new Viajes();
                            viaje.CalificacionConductor = 0;
                            viaje.Precio = decimal.Parse(costo);
                            viaje.FechaAlta = DateTime.Today;
                            viaje.FechaViaje = DateTime.Parse(fechaViaje).AddDays(i);
                            if (!ViajeValido(usuarioLogueado, viaje.FechaViaje.ToString(), duracion))
                            {
                                return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
                            }
                            viaje.Periodico = long.Parse(tipo);
                            viaje.Duracion = int.Parse(duracion);
                            viaje.Conductor = usuarioLogueado;
                            viaje.Auto = (Autos)session.Get(typeof(Autos), long.Parse(auto));
                            viaje.Origen = (Localidades)session.Get(typeof(Localidades), long.Parse(origen));
                            viaje.Destino = (Localidades)session.Get(typeof(Localidades), long.Parse(destino));
                            session.Save(viaje);
                        }
                    }
                    else if(tipo.Equals("2"))
                    {
                        for (int i = 0; i < int.Parse(cantidadSemanas); i++)
                        {
                            
                            Viajes viaje = new Viajes();
                            viaje.CalificacionConductor = 0;
                            viaje.Precio = decimal.Parse(costo);
                            viaje.FechaAlta = DateTime.Today;
                            viaje.FechaViaje = DateTime.Parse(fechaViaje).AddDays(7*i);
                            if (!ViajeValido(usuarioLogueado, viaje.FechaViaje.ToString(), duracion))
                            {
                                return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
                            }
                            viaje.Periodico = long.Parse(tipo);
                            viaje.Duracion = int.Parse(duracion);
                            viaje.Conductor = usuarioLogueado;
                            viaje.Auto = (Autos)session.Get(typeof(Autos), long.Parse(auto));
                            viaje.Origen = (Localidades)session.Get(typeof(Localidades), long.Parse(origen));
                            viaje.Destino = (Localidades)session.Get(typeof(Localidades), long.Parse(destino));
                            session.Save(viaje);
                        }
                    }
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