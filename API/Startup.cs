using API.Extensions;
using API.Helpers;
using API.MiddleWare;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StoreContext>(x => 
            {
                x.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddApplicationServices(); //ApplicationServicesExtension.cs class

            services.AddSwaggerDocumentation(); //SwaggerServicesExtension.cs class

            services.AddIdentityServices(_config); // IdentityServiceExtensions.cs class

            // Adding CORS --- CROSS ORIGIN RESOURCES SHARING
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);

                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddDbContext<AppIdentityDbContext>(options => 
            {
                options.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // comments this if statements & use our customized
            // exceptionMiddleware statement below
            //*****************************************
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy"); // Implements CORS

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseSwaggerDocumentation();  // SwaggerServicesExtension

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
