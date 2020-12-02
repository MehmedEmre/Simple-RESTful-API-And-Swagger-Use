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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace HotelFinder.API
{

    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(option => option.AddDefaultPolicy(builder => {

                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                // ‘AllowAnyHeader’, ‘AllowAnyMethod’ ve ‘AllowAnyOrigin’ metotlarýyla tüm clientlardan gelecek isteklere eriþim izni verilmiþtir.
            }));
  
            //JWT Token servisini uygulamaya entegre edelim
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => {
                
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    //Request isteðinde gelen tokenýn ValidAudience bilgisinin doðruluðunun kontrol edilip edilmemesi kýsmý. Eðer bu özellik false olursa tokenýn ValidAudience deðerinin bir önemi yoktur.
                    ValidateIssuer = true,
                    //Request isteðinde gelen tokenýn ValidIssuer bilgisinin doðruluðunun kontrol edilip edilmemesi kýsmý. Eðer bu özellik false olursa tokenýn ValidIssuer deðerinin bir önemi yoktur.
                    ValidateLifetime = true,
                    //‘ValidateLifetime’ ile token deðerinin kullaným süresi doðrulamasýný aktifleþtirdik.
                    //LifeTime Tokenýn ömrünün(expires) kullanýlýp kullanýlmayacaðýnýn belirlendiði kýsýmdýr. Eðer bu özellik false olursa tokenýn ömrünün bir önemi yoktur.
                    ValidateIssuerSigningKey = true,
                    //‘ValidateIssuerSigningKey’ ile token deðerinin bu uygulamaya ait olup olmadýðýný anlamamýzý saðlayan Security Key doðrulamasýný aktifleþtirdik.
                    ValidIssuer = "www.abc.com",
                    //Issuer Oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr. Örneðin; “www.myapi.com”
                    ValidAudience = "www.cba.com",
                    //Audience Oluþturulacak token deðerini kimlerin / hangi originlerin / sitelerin kullanacaðýný belirlediðimiz alandýr.Örneðin; “www.bilmemne.com”
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hey. I am a secret Key. Sooo Who are you.....")),
                    //‘IssuerSigningKey’ ile Security Key doðrulamasý için SymmetricSecurityKey nesnesi aracýlýðýyla mevcut keyi belirtiyoruz.
                    //SigningKey  Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulamasýdýr.
                    ClockSkew = TimeSpan.Zero,
                    //‘ClockSkew’ ile TimeSpan.Zero deðeri ile token süresinin üzerine ekstra bir zaman eklemeksizin sýfýr deðerini belirtiyoruz.
                    //ClockSkew Üretilecek token deðerinin expire süresinin belirtildiði deðer kadar uzatýlmasýný saðlayan özelliktir
                    //Tokenýn süresi default olarak 5 dakikadýr.
                };

                option.Events = new JwtBearerEvents
                {
                    OnTokenValidated = _ =>
                    {
                        //Eðer token bilgisi doðruysa buraya düþecek.
                        //Gerekirse burada gelen token içerisindeki çeþitli bilgilere göre doðrulam yapýlabilir.
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = _ =>
                    {
                        // Eðer Token bilgisi yanlýþsa buraya düþecek.
                        Console.WriteLine("Exception: {0}", _.Exception.Message);
                        return Task.CompletedTask;
                    }
                };


            });
            

            services.AddSingleton<IHotelService, HotelManager>();//Constructorda  IHotelService istiyorsa  HotelManager üret
            services.AddSingleton<IHotelRepository, HotelRepository>();
            services.AddSingleton<IUserService, UserManager>();
            services.AddSingleton<IUserRepository, UserRepository>();

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


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //UserRouting Ve UseAuthentication metodlarý arasýnda olmalýdýr
            //**********************
            app.UseCors();
            //**********************

            //JWT için gerekli middleware'lar
            //**********************

            app.UseAuthentication();
            app.UseAuthorization();// app.UseRouting(); ve app.UseEndpoints(); arasýnda olmalýdýr.

            //**********************


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
