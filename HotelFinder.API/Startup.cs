using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotelFinder.Buisness.Concrete;
using HotelFinder.Buisness.Abstract;
using HotelFinder.DataAccess.Concrete;
using HotelFinder.DataAccess.Abstract;

namespace HotelFinder.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IHotelService, HotelManager>();//Constructorda  IHotelService istiyorsa  HotelManager üret
            services.AddSingleton<IHotelRepository, HotelRepository>();

            //swagger enregrasyonu
            services.AddSwaggerDocument(config =>
                config.PostProcess = (doc =>
                {
                    doc.Info.Title = "All Hotel Api";
                    doc.Info.Version = "1.0.0";
                    doc.Info.Contact = new NSwag.OpenApiContact()
                    {
                        Name = "Mehmed Emre",
                        Email = "mehmedemreakdin@hotmail.com"

                    };
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger enregrasyonu
            app.UseOpenApi();
            app.UseSwaggerUi3();

        }
    }
}
