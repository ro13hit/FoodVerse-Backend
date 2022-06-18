using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic.Helper
{
    public class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionMessageAsync(context, e).ConfigureAwait(false);
            }
        }

        #region Private Methods
        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statuscode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statuscode,
                ErrorMessage = exception.Message
            });
            context.Response.StatusCode = statuscode;
            return context.Response.WriteAsync(result);
        }
        #endregion
    }
}
