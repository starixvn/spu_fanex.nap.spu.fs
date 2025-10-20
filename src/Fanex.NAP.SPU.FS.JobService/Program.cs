namespace Fanex.NAP.SPU.FS.JobService
{
    using Fanex.Logging;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                LogService.Info(string.Format("{0} has been started at {1}", typeof(Program).FullName, DateTimeOffset.Now));
                LogService.Error(string.Format("{0} has been started at {1}", typeof(Program).FullName, DateTimeOffset.Now));
                host.Run();
            }
            catch (Exception ex)
            {
                LogService.Error(string.Format("{0} has been started at {1} fail", typeof(Program).FullName, DateTimeOffset.Now), ex);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5000");

                    webBuilder
                    .UseStartup<Startup>();
                });
        }

    }
}
