using Core;
using Data;
using Data.Repository;
using Ninject;
using Ninject.Web.Common;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCTut
{
    //public class MvcApplication : System.Web.HttpApplication
    //{
    //    protected void Application_Start()
    //    {
    //        AreaRegistration.RegisterAllAreas();
    //        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        BundleConfig.RegisterBundles(BundleTable.Bundles);
    //        SetCurrentBusinessStatus();
    //    }

    //    public void SetCurrentBusinessStatus()
    //    {
    //        var eodRepository = new Repository<EOD>();
    //        var eod = eodRepository.GetAll().FirstOrDefault();
    //        if (eod != null)
    //        {
    //            IsBusinessOpen = eod.IsOpen;
    //        }

    //    }
    //    //public void CloseBusiness()
    //    //{
    //    //    EOD Eod = new EOD();
    //    //    Eod.IsOpen = false;

    //    //}

    //    public static bool IsBusinessOpen { get; set; }


    //}
    public class MvcApplication : NinjectHttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SetCurrentBusinessStatus();
        }

        public void SetCurrentBusinessStatus()
        {
            var eodRepository = new Repository<EOD>();
            var eod = eodRepository.GetAll().FirstOrDefault();
            if (eod != null)
            {
                IsBusinessOpen = eod.IsOpen;
            }

        }
        //public void CloseBusiness()
        //{
        //    EOD Eod = new EOD();
        //    Eod.IsOpen = false;

        //}

        public static bool IsBusinessOpen { get; set; }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
           // kernel.Load(Assembly.GetExecutingAssembly);

            kernel.Load(new RepositoryModule());
            kernel.Bind<IBlogRepository>().To<BlogRepository>();

            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            base.OnApplicationStarted();
        }
    }
}
