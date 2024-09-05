using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Interview_API.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    string? errorMessage = contextFeature?.Error.Message;
                    if (contextFeature is not null)
                    {
                        if (contextFeature.Error.InnerException is not null)
                        {
                            if (contextFeature.Error.InnerException.Message.Contains(
                                "DELETE statement conflicted with the REFERENCE constraint"))
                                await context.Response.WriteAsync("You are not allowed to Delete this Record as it has Child Records available in Database");
                            else
                                await context.Response.WriteAsync(contextFeature.Error.InnerException.Message);
                        }
                        else
                            await context.Response.WriteAsync(contextFeature.Error.Message);
                        //DELETE statement conflicted with the REFERENCE constraint
                    }
                });
            });
        }
    }
}