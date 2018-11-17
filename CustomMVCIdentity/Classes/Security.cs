using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityManagement.Entities;
using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity.Owin;

namespace CustomMVCIdentity
{
    public class Security
    {
        public static void CheckIsUserValid( long user_id )
        {
            //fetch user
            //ApplicationUser oUser = SignInManager.UserManager.FindByNameAsync( objLogin.UserName );
            //{
            //    if( ( oUser == null ) || ( oUser.Id == null ) )
            //    {
            //        throw new Exception( "User was permanently removed from the system" );
            //    }

            //    if( oUser.Status == EnumUserStatus.Banned )
            //    {
            //        throw new Exception( "User has been banned" );
            //    }

            //    if( oUser.Status == EnumUserStatus.Closed)
            //    {
            //        throw new Exception( "User has been closed" );
            //    }
            //}
        }
    }
}