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
                    var viajes = session.QueryOver<Viajes>();
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