using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace AspnetCoreAPI
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
            services.AddHealthChecks();

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });


            services.AddMassTransit(x =>
            {
                x.AddConsumer<ValueEnteredEventConsumer>();
                x.SetKebabCaseEndpointNameFormatter();

                 x.UsingRabbitMq((context, cfg) =>
                {
                    // cfg.ReceiveEndpoint("event-listener", e =>
                    // {
                    //     e.ConfigureConsumer<EventConsumer>(context);
                    // });
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspnetCoreAPI", Version = "v1" });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspnetCoreAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Health
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());
            });
        }
    }

    class ValueEnteredEventConsumer  :
        IConsumer<ValueEntered>
    {
        ILogger<ValueEnteredEventConsumer> _logger;

        public ValueEnteredEventConsumer(ILogger<ValueEnteredEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Value);
        }
    }
}
