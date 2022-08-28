using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepoLayer.Context;
using RepoLayer.Interfaces;
using RepoLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // To Connect With Front-End

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:5500").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddDbContext<FundooContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:FundooDB"]));
            services.AddControllers();
            services.AddTransient<IUserBL, UserBL>();
            services.AddTransient<IUserRL, UserRL>();
            services.AddTransient<INoteBL, NoteBL>();
            services.AddTransient<INoteRL, NoteRL>();
            services.AddTransient<ICollabBL, CollabBL>();
            services.AddTransient<ICollabRL, CollabRL>();
            services.AddTransient<ILabelBL, LabelBL>();
            services.AddTransient<ILabelRL, LabelRL>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {

                    Scheme = "bearer",

                    BearerFormat = "JWT",

                    Name = "JWT Authentication",

                    In = ParameterLocation.Header,

                    Type = SecuritySchemeType.Http,

                    Description = "enter JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,

                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                 { jwtSecurityScheme, Array.Empty<string>() }
                });

            });


            var tokenKey = Configuration.GetValue<string>("Jwt:Key");

            var key = Encoding.ASCII.GetBytes(tokenKey);




            services.AddAuthentication(x =>

            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })

            .AddJwtBearer(x =>

            {

                x.RequireHttpsMetadata = false;

                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters

                {

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,

                    ValidateAudience = false

                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fundoo");
            });
        }
    }
}
