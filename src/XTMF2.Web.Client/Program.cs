using System;
using Microsoft.AspNetCore.Blazor.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
namespace XTMF2.Web.Client {
    public class Program {
        public static void Main (string[] args) {

            SelfLog.Enable (m => Console.Error.WriteLine (m));

            Log.Logger = new LoggerConfiguration ()
                .MinimumLevel.Debug ()
                .WriteTo.BrowserConsole ()
                .CreateLogger ();

            Log.Debug ("Hello, browser!");
            Log.Information ("Received cat {@Response} ", new { Username = "example", Cats = 7 });

            CreateHostBuilder (args).Build ().Run ();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder (string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder ()
            .UseBlazorStartup<Startup> ();
    }
}