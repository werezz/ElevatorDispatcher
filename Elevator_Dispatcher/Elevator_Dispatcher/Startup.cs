using Elevator_Dispatcher.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevator_Dispatcher
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
            services.AddControllers();

            services.AddScoped<IElevatorRoutingService, ElevatorRoutingService>();
            services.AddScoped<IElevatorActionLoggingService, ElevatorActionLoggingService>();
            services.AddScoped<IElevatorStatusService, ElevatorStatusService>();
            services.AddScoped<IElevatorControlService, ElevatorControlService>();
            services.AddScoped<IElevatorRoutingValidationService, ElevatorRoutingValidationService>();

            services.AddSingleton<IElevatorPoolService, ElevatorPoolService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
