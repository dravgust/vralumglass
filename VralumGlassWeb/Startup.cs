using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VralumGlassWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vralumglass.Core;
using Vralumglass.Dropbox;

namespace VralumGlassWeb
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.Configure<CookiePolicyOptions>(options =>
			//{
			//	// This lambda determines whether user consent for non-essential cookies is needed for a given request.
			//	options.CheckConsentNeeded = context => true;
			//	options.MinimumSameSitePolicy = SameSiteMode.None;
			//});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

			services.AddDefaultIdentity<IdentityUser>()
				.AddDefaultUI(UIFramework.Bootstrap4)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
			services.AddTransient<IEmailSender, MessageSender>();

			services.Configure<FileStorageSettings>(Configuration.GetSection("FileStorageSettings"));
			services.AddTransient<IFileStorage, DropboxStorage>();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseStaticFiles();
			//app.UseCookiePolicy();

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});
			
			app.UseAuthentication();

			app.UseMvc();
		}
	}
}
