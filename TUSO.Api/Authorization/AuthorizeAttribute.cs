using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

namespace TUSO.Authorization
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    //{
    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        var account = (UserInfo)context.HttpContext.Items["User"];
    //        if (account == null)
    //        {
    //            // not logged in
    //            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
    //        }
    //    }
    //}


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {


        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var account = (UserAccount)filterContext.HttpContext.Items["User"];
            if (account == null)
            {
                // not logged in
                filterContext.Result = new JsonResult(new { code = StatusCodes.Status401Unauthorized, status=false,  message = "Unauthorized", data = "" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
         
            //if (filterContext != null)
            //{
            //    Microsoft.Extensions.Primitives.StringValues authTokens;
            //    filterContext.HttpContext.Request.Headers.TryGetValue("authToken", out authTokens);

            //    var _token = authTokens.FirstOrDefault();

            //    if (_token != null)
            //    {
            //        string authToken = _token;
            //        if (authToken != null)
            //        {
            //            if (IsValidToken(authToken))
            //            {
            //                filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
            //                filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

            //                filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

            //                return;
            //            }
            //            else
            //            {
            //                filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
            //                filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

            //                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //                filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
            //                filterContext.Result = new JsonResult("NotAuthorized")
            //                {
            //                    Value = new
            //                    {
            //                        Status = "Error",
            //                        Message = "Invalid Token"
            //                    },
            //                };
            //            }

            //        }

            //    }
            //    else
            //    {
            //        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
            //        filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
            //        filterContext.Result = new JsonResult("Please Provide authToken")
            //        {
            //            Value = new
            //            {
            //                Status = "Error",
            //                Message = "Please Provide authToken"
            //            },
            //        };
            //    }
            //}
        }

        public bool IsValidToken(string authToken)
        {
            //validate Token here  
            return true;
        }
    }

}
