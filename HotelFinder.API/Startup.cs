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
                // �AllowAnyHeader�, �AllowAnyMethod� ve �AllowAnyOrigin� metotlar�yla t�m clientlardan gelecek isteklere eri�im izni verilmi�tir.
            }));
  
            //JWT Token servisini uygulamaya entegre edelim
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => {
                
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    //Request iste�inde gelen token�n ValidAudience bilgisinin do�rulu�unun kontrol edilip edilmemesi k�sm�. E�er bu �zellik false olursa token�n ValidAudience de�erinin bir �nemi yoktur.
                    ValidateIssuer = true,
                    //Request iste�inde gelen token�n ValidIssuer bilgisinin do�rulu�unun kontrol edilip edilmemesi k�sm�. E�er bu �zellik false olursa token�n ValidIssuer de�erinin bir �nemi yoktur.
                    ValidateLifetime = true,
                    //�ValidateLifetime� ile token de�erinin kullan�m s�resi do�rulamas�n� aktifle�tirdik.
                    //LifeTime Token�n �mr�n�n(expires) kullan�l�p kullan�lmayaca��n�n belirlendi�i k�s�md�r. E�er bu �zellik false olursa token�n �mr�n�n bir �nemi yoktur.
                    ValidateIssuerSigningKey = true,
                    //�ValidateIssuerSigningKey� ile token de�erinin bu uygulamaya ait olup olmad���n� anlamam�z� sa�layan Security Key do�rulamas�n� aktifle�tirdik.
                    ValidIssuer = "www.abc.com",
                    //Issuer Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r. �rne�in; �www.myapi.com�
                    ValidAudience = "www.cba.com",
                    //Audience Olu�turulacak token de�erini kimlerin / hangi originlerin / sitelerin kullanaca��n� belirledi�imiz aland�r.�rne�in; �www.bilmemne.com�
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hey. I am a secret Key. Sooo Who are you.....")),
                    //�IssuerSigningKey� ile Security Key do�rulamas� i�in SymmetricSecurityKey nesnesi arac�l���yla mevcut keyi belirtiyoruz.
                    //SigningKey  �retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security key verisinin do�rulamas�d�r.
                    ClockSkew = TimeSpan.Zero,
                    //�ClockSkew� ile TimeSpan.Zero de�eri ile token s�resinin �zerine ekstra bir zaman eklemeksizin s�f�r de�erini belirtiyoruz.
                    //ClockSkew �retilecek token de�erinin expire s�resinin belirtildi�i de�er kadar uzat�lmas�n� sa�layan �zelliktir
                    //Token�n s�resi default olarak 5 dakikad�r.
                };

                option.Events = new JwtBearerEvents
                {
                    OnTokenValidated = _ =>
                    {
                        //E�er token bilgisi do�ruysa buraya d��ecek.
                        //Gerekirse burada gelen token i�erisindeki �e�itli bilgilere g�re do�rulam yap�labilir.
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = _ =>
                    {
                        // E�er Token bilgisi yanl��sa buraya d��ecek.
                        Console.WriteLine("Exception: {0}", _.Exception.Message);
                        return Task.CompletedTask;
                    }
                };


            });
            

            services.AddSingleton<IHotelService, HotelManager>();//Constructorda  IHotelService istiyorsa  HotelManager �ret
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

            //UserRouting Ve UseAuthentication metodlar� aras�nda olmal�d�r
            //**********************
            app.UseCors();
            //**********************

            //JWT i�in gerekli middleware'lar
            //**********************

            app.UseAuthentication();
            app.UseAuthorization();// app.UseRouting(); ve app.UseEndpoints(); aras�nda olmal�d�r.

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
