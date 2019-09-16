using System.Web.Mvc;
using System.Web.Routing;

namespace MVCTut
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Posts",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "Posts", id = UrlParameter.Optional }
            );
            routes.MapRoute("Category", "Category/{category}",
                new { controller = "Blog", action = "Category" }
                            );
            routes.MapRoute("Tag", "Tag/{tag}",
                new { controller = "Blog", action = "Tag" }
                            );
            routes.MapRoute("Post", "Archive/{year}/{month}/{title}",
                 new { controller = "Blog", action = "Post" }
                            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
