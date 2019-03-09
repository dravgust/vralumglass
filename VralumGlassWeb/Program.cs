using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace VralumGlassWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
		    var logger = NLogBuilder.ConfigureNLog("/app/nlog.config").GetCurrentClassLogger();
		    try
		    {
		        CreateWebHostBuilder(args).Build().Run();
            }
		    catch (Exception e)
		    {
                logger.Error(e.Message);
		    }
		    finally
		    {
		        LogManager.Shutdown();
		    }
		}

	    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
	        WebHost.CreateDefaultBuilder(args)
	            .UseStartup<Startup>()
	            .ConfigureLogging(log =>
	            {
	                log.ClearProviders();
	                log.SetMinimumLevel(LogLevel.Trace);
	            })
	            .UseNLog();
	}
}
