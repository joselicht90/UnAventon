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

        public JsonResult GuardarComentario(long idViaje, string comentario, string idComentario = null)
        {
            try
            {
                ISession session = NHibernate.NHibernateHelper.GetCurrentSession();
                if (idComentario == null || idComentario == "")
                {
                    
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    Comentarios comentarioNuevo = new Comentarios();
                    comentarioNuevo.Texto = comentario;
                    comentarioNuevo.ViajeId = viaje.Id;
                    session.Save(comentarioNuevo);
                }
                else
                {
                    Comentarios coment = (Comentarios)session.Get(typeof(Comentarios), long.Parse(idComentario));
                    coment.Respuesta = comentario;
                    session.Update(coment);
                }
                session.Flush();
                return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar comentar el viaje." }, JsonRequestBehavior.AllowGet);

            }
        }

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
                    if (cantidadPasajeros == viaje.Auto.Asientos)
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

        public JsonResult CerrarPostulaciones(long idViaje)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    if(DateTime.Today < viaje.FechaViaje)
                    {
                        viaje.Estado = "EN_CURSO";
                    }
                    else
                    {
                        return Json(new { mensaje = "La fecha de viaje ya ha expirado." }, JsonRequestBehavior.AllowGet);
                    }
                    session.Update(viaje);
                    transaction.Commit();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar aceptar el pasajero." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Calificar(long idViaje, long idPasajero, long puntaje)
        {
            if(idPasajero != -1)
            {
                try
                {
                    ISession session = NHibernateHelper.GetCurrentSession();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                        Pasajeros pasajero = viaje.Pasajeros.Where(x => x.Id == idPasajero).Single();
                        pasajero.Estado = "Calificado";
                        pasajero.Usuario.CReputacion += puntaje;
                        session.Update(pasajero);
                        transaction.Commit();
                        return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { mensaje = "Ha ocurrido un error al intentar calificar al pasajero." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                try
                {
                    ISession session = NHibernateHelper.GetCurrentSession();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                        viaje.CalificacionConductor = puntaje;
                        viaje.Conductor.PReputacion += puntaje;
                        session.Update(viaje);
                        transaction.Commit();
                        return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { mensaje = "Ha ocurrido un error al intentar calificar al conductor." }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult FinalizarViaje(long idViaje)
        {
            try
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    if (viaje.Pasajeros.Any(x => !x.Estado.Equals("Calificado")))
                    {
                        return Json(new { mensaje = "Debe calificar a sus pasajeros para poder finalizar el viaje." }, JsonRequestBehavior.AllowGet);
                    }
                    viaje.Estado = "FINALIZADO";
                    session.Update(viaje);
                    transaction.Commit();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar aceptar el pasajero." }, JsonRequestBehavior.AllowGet);
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
                    if (pasajero.Estado.Equals("Aceptado"))
                    {
                        Usuarios usuario = viaje.Conductor;
                        usuario.PReputacion--;
                        session.Update(usuario);
                    }
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
                    if (pasajero.Estado.Equals("Aceptado") && pasajero.Usuario.CReputacion > 0)
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
                    if (viaje.Pasajeros.Any(x => x.Estado.Equals("Aceptado")) && viaje.Conductor.PReputacion > 0)
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
        private bool ViajeValidoEditar(Usuarios usuarioLogueado, string fechaViaje, string duracion, long idViaje)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            IList<Viajes> viajesUsuario = session.QueryOver<Viajes>().Where(x => x.Conductor.Id == usuarioLogueado.Id).List();
            viajesUsuario = viajesUsuario.Where(x => x.Id != idViaje).ToList();
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

        public JsonResult EditarViaje(string origen, string destino, string auto, string costo, string duracion, string fechaViaje, long idViaje)
        {
            Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
            try
            {
                if (costo.Contains('.') || costo.Contains(','))
                {
                    return Json(new { mensaje = "Debe ingresar un costo en valores enteros." }, JsonRequestBehavior.AllowGet);

                }
                if (duracion.Contains('.') || duracion.Contains(','))
                {
                    return Json(new { mensaje = "Debe ingresar un aproximado de minutos que dura el viaje en valores enteros." }, JsonRequestBehavior.AllowGet);

                }
                if (auto.Equals(""))
                {
                    return Json(new { mensaje = "El viaje debe contas con un auto seleccionado." }, JsonRequestBehavior.AllowGet);

                }
                if (DateTime.Parse(fechaViaje) < DateTime.Today)
                {
                    return Json(new { mensaje = "El viaje no puede ser programado para una fecha anterior al dia de hoy." }, JsonRequestBehavior.AllowGet);

                }
                ISession session = NHibernateHelper.GetCurrentSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Viajes viaje = (Viajes)session.Get(typeof(Viajes), idViaje);
                    viaje.Precio = decimal.Parse(costo);
                    viaje.FechaViaje = DateTime.Parse(fechaViaje);
                    if (!ViajeValidoEditar(usuarioLogueado, viaje.FechaViaje.ToString(), duracion, idViaje))
                    {
                        return Json(new { mensaje = "Usted ya tiene un viaje planificado en ese rango horario." }, JsonRequestBehavior.AllowGet);
                    }
                    viaje.Duracion = int.Parse(duracion);
                    viaje.Conductor = usuarioLogueado;
                    viaje.Auto = (Autos)session.Get(typeof(Autos), long.Parse(auto));
                    viaje.Origen = (Localidades)session.Get(typeof(Localidades), long.Parse(origen));
                    viaje.Destino = (Localidades)session.Get(typeof(Localidades), long.Parse(destino));
                    session.Update(viaje);
                    transaction.Commit();
                    return Json(new { mensaje = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { mensaje = "Ha ocurrido un error al intentar guardar el nuevo viaje." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GuardarViaje(string tipo, string origen, string destino, string auto, string costo, string duracion, string fechaViaje, string cantidadSemanas)
        {
            Usuarios usuarioLogueado = (Usuarios)Session["UsuarioLogueado"];
            try
            {
                if (costo.Contains('.') || costo.Contains(','))
                {
                    return Json(new { mensaje = "Debe ingresar un costo en valores enteros." }, JsonRequestBehavior.AllowGet);

                }
                if (duracion.Contains('.') || duracion.Contains(','))
                {
                    return Json(new { mensaje = "Debe ingresar un aproximado de minutos que dura el viaje en valores enteros." }, JsonRequestBehavior.AllowGet);

                }
                if (auto.Equals(""))
                {
                    return Json(new { mensaje = "El viaje debe contas con un auto seleccionado." }, JsonRequestBehavior.AllowGet);

                }
                if (DateTime.Parse(fechaViaje) < DateTime.Today)
                {
                    return Json(new { mensaje = "El viaje no puede ser programado para una fecha anterior al dia de hoy." }, JsonRequestBehavior.AllowGet);

                }
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
                        viaje.Estado = "ABIERTO";
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
                        for (int i = 0; i < dias; i++)
                        {
                            Viajes viaje = new Viajes();
                            viaje.CalificacionConductor = 0;
                            viaje.Precio = decimal.Parse(costo);
                            viaje.FechaAlta = DateTime.Today;
                            viaje.Estado = "ABIERTO";
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
                    else if (tipo.Equals("2"))
                    {
                        for (int i = 0; i < int.Parse(cantidadSemanas); i++)
                        {

                            Viajes viaje = new Viajes();
                            viaje.CalificacionConductor = 0;
                            viaje.Precio = decimal.Parse(costo);
                            viaje.FechaAlta = DateTime.Today;
                            viaje.Estado = "ABIERTO";
                            viaje.FechaViaje = DateTime.Parse(fechaViaje).AddDays(7 * i);
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
                return Json(new { mensaje = "Ha ocurrido un error al intentar guardar el nuevo viaje." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}