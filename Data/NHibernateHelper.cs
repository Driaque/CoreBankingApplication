using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Map;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Configuration;

namespace Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        public static ISession Session => _sessionFactory.OpenSession();
        //GetSession();

        //private static ISession theSession { get; set; }

        //private static ISession GetSession()
        //{
        //    if (theSession == null || !theSession.IsOpen)
        //        theSession = _sessionFactory.OpenSession();

        //    return theSession;
        //}

        static NHibernateHelper()
        {
            InitializeSessionFactory();
        }

        public static void InitializeSessionFactory()
        {
            var hibernateConnection = MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.AppSettings["HibernateDbConnection"]);

            _sessionFactory = Fluently.Configure()
                                      .Database(hibernateConnection.ShowSql())
                                      .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BranchMap>())
                                      .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                      .BuildSessionFactory();
            return;

        }
    }
}
