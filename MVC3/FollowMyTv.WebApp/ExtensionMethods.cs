using System.IO;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using FollowMyTv.DomainLayer;
using FollowMyTv.WebApp.Controllers;
using FollowMyTv.WebApp.Models;

namespace FollowMyTv.WebApp
{
    public static class ExtensionMethods
    {
        public static bool IsAdministrator( this IPrincipal principal )
        {
            return principal.IsInRole( FollowMyTvRoles.ADMINISTRATOR );
        }

        public static bool IsAuthenticatedUser( this IPrincipal principal )
        {
            return principal.IsInRole( FollowMyTvRoles.AUTH_USER );
        }

        public static bool IsAnonymousUser( this IPrincipal principal )
        {
            return !principal.Identity.IsAuthenticated;
        }

        public static MvcHtmlString ShowShowsActionsForCurrentUser( this HtmlHelper helper, Show s )
        {
            return helper.Partial( "Showactions", s );
        }

        public static EpisodeModel ToEpisodeModel( this Episode episode, string showName, int seasonNumber )
        {
            return new EpisodeModel
                       {
                           Name = episode.Title,
                           Number = episode.Number,
                           Score = episode.Score,
                           ShowName = showName,
                           SeasonNumber = seasonNumber,
                           Synopsis = episode.Synopsis,
                           Duration = episode.Duration
                       };
        }

        public static Episode ToEpisodeDomainEntity( this EpisodeModel model )
        {
            return new Episode { Title = model.Name, Number = model.Number, Synopsis = model.Synopsis, Score = model.Score, Duration = model.Duration };
        }

        public static ShowModel ToShowModel( this Show showObj )
        {
            return new ShowModel { Name = showObj.Name, Description = showObj.Description };
        }

        public static string RenderPartialViewToString( this Controller controller, string viewName, object model = null )
        {
            if ( string.IsNullOrEmpty( viewName ) )
                viewName = controller.ControllerContext.RouteData.GetRequiredString( "action" );
            controller.ViewData.Model = model;

            using ( StringWriter sw = new StringWriter() )
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView( controller.ControllerContext, viewName );
                ViewContext viewContext = new ViewContext( controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw );
                viewResult.View.Render( viewContext, sw );

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}