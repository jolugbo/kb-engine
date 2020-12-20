using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using jolugbokb.Utills;
using jolugbokb.Data;
using Microsoft.EntityFrameworkCore;
using jolugbokb.middleware;

namespace jolugbokb
{
    public class Startup
    {
        Converters converters = new Converters();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.WriteLine(converters.DecodeEncrypt(Configuration["DBName"]));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<Converters>();
            services.AddTransient<UserMiddleware>();
            services.AddTransient<UtillMiddleware>();
            string cs = converters.DecodeEncrypt(Configuration["ConnectionString"]).Replace("DBName", converters.DecodeEncrypt(Configuration["DBName"]));
            services.AddDbContext<KBDBContext>(option => option.UseSqlServer(cs));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, KBDBContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            context.Database.EnsureCreated();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
