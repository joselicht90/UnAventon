using System;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate;
using UnAventon.Models;

namespace UnAventon.NHibernate
{
    public sealed class NHibernateHelper
    {
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory _sessionFactory;

        static NHibernateHelper()
        {
            var cfg = new Configuration();
            cfg.Configure();
            //_sessionFactory = new Configuration().Configure().BuildSessionFactory();
            _sessionFactory = Fluently.Configure(cfg)
                .Mappings(m =>
                {
                    //m.HbmMappings.AddFromAssemblyOf<Viajes>();
                    m.FluentMappings.AddFromAssemblyOf<ViajesMap>();
                }).BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            
            var context = HttpContext.Current;
            var currentSession = context.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
                currentSession = _sessionFactory.OpenSession();
                context.Items[CurrentSessionKey] = currentSession;
            }

            return currentSession;
        }

        public static void CloseSession()
        {
            var context = HttpContext.Current;
            var currentSession = context.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
                // No current session
                return;
            }

            currentSession.Close();
            context.Items.Remove(CurrentSessionKey);
        }

        public static void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
            }
        }
    }
}