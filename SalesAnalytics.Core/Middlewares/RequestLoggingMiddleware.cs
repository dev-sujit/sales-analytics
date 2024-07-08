using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        //private readonly ISalesDbContext _context;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger
            //, ISalesDbContext context
            )
        {
            _next = next;
            _logger = logger;
            //_context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Log request
                _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

                // Capture the response
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    // Log response
                    responseBody.Seek(0, SeekOrigin.Begin);
                    string responseBodyContent = await new StreamReader(responseBody).ReadToEndAsync();

                    if (!string.IsNullOrEmpty(responseBodyContent))
                    {
                        //_logger.LogInformation($"Response: {context.Response.StatusCode}, Body: {responseBodyContent}");

                        //// Save to database
                        //var apiLog = new ApiLog
                        //{
                        //    Timestamp = DateTime.Now,
                        //    Method = context.Request.Method,
                        //    Path = context.Request.Path,
                        //    Request = "", // Add your request details if needed
                        //    Response = responseBodyContent,
                        //    ErrorMessage = "", // Set error message if applicable
                        //    StatusCode = context.Response.StatusCode
                        //};

                        //_context.ApiLogs.Add(apiLog);
                        //await _context.SaveChangesAsync(CancellationToken.None);
                    }
                    else
                    {
                        _logger.LogInformation($"Response: {context.Response.StatusCode}");
                    }

                    //var apiLogs = new ApiLog()
                    //{
                    //    Timestamp = DateTime.Now,
                    //    Method = context.Request.Method,
                    //    Path = context.Request.Path,
                    //    Request = "context.Request",
                    //    Response = responseBodyContent,
                    //    ErrorMessage = "",
                    //    StatusCode = 200
                    //};

                    //_context.ApiLogs.Add(apiLogs);

                    //await _context.SaveChangesAsync(CancellationToken.None);

                    // Copy the captured response back to the original stream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                // Log errors
                _logger.LogError($"Exception: {ex.Message}");
                throw; // Re-throw the exception to be handled by the global exception handler
            }
        }
    }
}
