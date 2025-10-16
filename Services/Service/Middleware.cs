using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;

namespace Casino.Services.Service
{
   public class Middleware
    {
        private readonly RequestDelegate next;
        public Middleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // var path = context.Request.Path;
            // if (path == "/Blackjack")
            //{

            var request = context.Request;
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                HttpRequestRewindExtensions.EnableBuffering(request);
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                //get body string here...
                var requestContent = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
            }

            var email = context.Request.Query["email"];
                Console.WriteLine(email);
            //}

               //await context.Response.WriteAsync("123");
            //else
              await next.Invoke(context);
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                 
            }

        }

    }
}
