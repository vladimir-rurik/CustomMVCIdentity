using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomMVCIdentity.Filters
{
	public class VerifyUserAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting( ActionExecutingContext filterContext )
		{
			if( HttpContext.Current.Session != null && !filterContext.IsChildAction ) //child requests (partial views) should be ignored, since we can't redirect from them
			{
				if( Shared.NextIsUserValidCheckTime < DateTime.Now )
				{
					//verify user
					try
					{
						Security.CheckIsUserValid( Shared.UserId );
					}
					catch( Exception ex )
					{
						//create redirect url
						filterContext.Result = new RedirectToRouteResult(
							new RouteValueDictionary( new { controller = "Login", action = "Logout", logout_reason = ex.Message } )
						);

						if( filterContext.HttpContext.Request.IsAjaxRequest() )
						{
							//for ajax requests return json
							RedirectToRouteResult result = filterContext.Result as RedirectToRouteResult;

							//ajax request - return redirect response
							filterContext.Result = new JsonResult
							{
								Data = new { Redirect = UrlHelper.GenerateUrl( result.RouteName, null, null, result.RouteValues, RouteTable.Routes, filterContext.RequestContext, false ) },
								JsonRequestBehavior = JsonRequestBehavior.AllowGet
							};
						}
						else
						{
							//for normal requests return redirect request
							filterContext.Result.ExecuteResult( filterContext.Controller.ControllerContext );
						}
					}
					Shared.ResetNextIsUserValidCheckTime();
				}

			}
		}
	}
}