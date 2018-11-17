using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CustomMVCIdentity
{
    public static class Shared
    {
        public static string Path
        {
            get
            {
                return AddSlash( System.Web.HttpContext.Current.Request.ApplicationPath );
            }
        }
        public static string AddSlash( string slashed )
        {
            if( slashed.EndsWith( "/" ) )
                return slashed;
            else
                return slashed + "/";
        }
        public static long UserId
        {
            get
            {
                long user_id = Convert.ToInt64( System.Web.HttpContext.Current.Items["user_id"] );
                if( user_id == 0 ) user_id = Convert.ToInt64( "0" + System.Web.HttpContext.Current.User.Identity.Name );
                if( user_id == 0 ) // for the situation where auth cookie is set but not reflected in identity yet (until next request)
                {
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if( authCookie == null ) return 0;
                    if( String.IsNullOrEmpty( authCookie.Value ) ) return 0;
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt( authCookie.Value );
                    user_id = Convert.ToInt64( ticket.Name );
                }
                return user_id;
            }
        }

        public static DateTime NextIsUserValidCheckTime
        {
            get
            {
                if( HttpContext.Current.Session["next_is_user_valid_check_time"] == null )
                {
                    HttpContext.Current.Session["next_is_user_valid_check_time"] = GetNextUserValidCheckTime;
                }
                return Convert.ToDateTime( HttpContext.Current.Session["next_is_user_valid_check_time"] );
            }

            set
            {
                HttpContext.Current.Session["next_is_user_valid_check_time"] = value;
            }
        }

        public static DateTime GetNextUserValidCheckTime
        {
            get
            {
                int seconds_interval = 10;
                return DateTime.Now.AddSeconds( seconds_interval );
            }
        }

        public static void ResetNextIsUserValidCheckTime()
        {
            NextIsUserValidCheckTime = GetNextUserValidCheckTime;
        }

        private static Dictionary<string, string> default_settings = new Dictionary<string, string> { { "APP_ID", "3" }, { "theme", "bootstrap" }, { "logout_reason", "" } };
        public static string APP_ID
        {
            get
            {
                string app_id = LoadSetting( "APP_ID" );
                if( app_id == "0" ) app_id = default_settings["APP_ID"];
                return app_id;
            }
            set { SaveSetting( "APP_ID", value ); }
        }
        public static string theme
        {
            get { return LoadSetting( "theme" ); }
            set { SaveSetting( "theme", value ); }
        }

        //when user is forced to logout a cookie is stored and displayed for the user next time
        public static string logout_reason
        {
            get { return LoadSetting( "logout_reason" ); }
            set { SaveSetting( "logout_reason", value ); }
        }

        private static void SaveSetting( string name, string value )
        {
            if( HttpContext.Current != null )
            {
                if( value == null ) value = default_settings[name];
                HttpContext.Current.Items[name] = value;
                HttpCookie cookie = new HttpCookie( name, value.ToString() );
                cookie.Path = Shared.Path;
                //cookie.Expires = DateTime.Now.AddYears(9);
                HttpContext.Current.Response.Cookies.Set( cookie );
            }
        }

        private static string LoadSetting( string name )
        {
            string default_value = default_settings[name];
            if( HttpContext.Current == null ) return default_value;
            string fromRequest = Coalesce( HttpContext.Current.Items[name] );
            if( fromRequest.Length > 0 ) return fromRequest;
            if( HttpContext.Current.Request.Cookies[name] != null ) fromRequest = HttpContext.Current.Request.Cookies[name].Value;
            if( fromRequest.Length > 0 ) return fromRequest;
            SaveSetting( name, default_value );
            return default_value;
        }

        public static string Coalesce( string maybenull )
        {
            if( maybenull == null ) return ""; else return maybenull;
        }

        public static string Coalesce( object maybenull )
        {
            if( maybenull == null ) return ""; else return maybenull.ToString();
        }

        public static IEnumerable Coalesce( IEnumerable maybenull )
        {
            if( maybenull == null ) return (IEnumerable)Activator.CreateInstance( maybenull.GetType() ); else return maybenull;
        }

        public static DataTable Coalesce( DataTable maybenull )
        {
            if( maybenull == null ) return new DataTable(); else return maybenull;
        }

        public static bool Coalesce( bool? maybenull )
        {
            if( maybenull == null ) return false; else return (bool)maybenull;
        }

        public static DateTime Coalesce( DateTime? maybenull )
        {
            if( maybenull == null ) return DateTime.MinValue; else return (DateTime)maybenull;
        }
    }
}