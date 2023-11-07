using System.Net;
using System.Text.Json;
using VillaApi.Controllers;
using VillaApi.Exceptions;
using VillaApi.Models;
using KeyNotFoundException = VillaApi.Exceptions.KeyNotFoundException;
using NotImplementedException = VillaApi.Exceptions.NotImplementedException;
using UnauthorizedAccessException = VillaApi.Exceptions.UnauthorizedAccessException;

namespace VillaApi.Root.Configurations
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message;

            var responseModel = new ResponseModel();

            var exceptionType = exception.GetType();
            //if (exceptionType == typeof(BadRequestException))
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
            //}
            //else if (exceptionType == typeof(NotFoundException))
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.NotFound;
            //}
            //else if (exceptionType == typeof(NotImplementedException))
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.NotImplemented;
            //}
            //else if (exceptionType == typeof(UnauthorizedAccessException))
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.Unauthorized;
            //}
            //else if (exceptionType == typeof(KeyNotFoundException))
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
            //}

            //else
            //{
            //    responseModel.ResponseMessage = exception.Message;
            //    responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
            //}
            (responseModel.ResponseMessage, responseModel.StatusCode) = exceptionType.Name switch
            {
                nameof(BadRequestException) => (exception.Message, (int)HttpStatusCode.BadRequest),
                nameof(NotFoundException) => (exception.Message, (int)HttpStatusCode.NotFound),
                nameof(NotImplementedException) => (exception.Message, (int)HttpStatusCode.NotImplemented),
                nameof(UnauthorizedAccessException) => (exception.Message, (int)HttpStatusCode.Unauthorized),
                nameof(KeyNotFoundException) => (exception.Message, (int)HttpStatusCode.BadRequest),
                _ => (exception.Message, (int)HttpStatusCode.InternalServerError)
            };
            var exceptionResult = JsonSerializer.Serialize(responseModel);
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
