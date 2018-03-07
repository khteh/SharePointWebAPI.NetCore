using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebAPI.NetCore.Models;

namespace WebAPI.NetCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SharePointContext>(opt => opt.UseInMemoryDatabase("SharePointInMemoryDatabase"));
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(i =>
            {
                i.SwaggerDoc("v1", new Info
                {
                    Version = "1.0",
                    Title = "SharePoint Web API",
                    Description = "SharePoint administration using .Net Core 2.0 and Microsoft SharePoint Client component library",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Teh Kok How", Email = "funcoolgeek@gmail.com", Url = "https://github.com/khteh" },
                    License = new License { Name = "Use under LICX", Url = "https://" }
                });
                // Set the comments path for the swagger JSON and UI
                i.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SharePointWebAPI.NetCore.xml"));
            });
            //services.AddAuthorization();
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SharePointContext>();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
            //    options.Audience = "http://localhost:52137";
            //    options.Authority = "http://localhost:52137";
            //});
            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseSwagger(); // Enable the middleware to serve generated Swagger as a JSON endpoint
            app.UseSwaggerUI(i => i.SwaggerEndpoint("/swagger/v1/swagger.json", "SharePoint API v1.0"));
            //app.UseAuthentication();
            app.UseMvc();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "api/{controller}/{action}/{id?}");
            //});
        }
    }
}
