using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RawRabbit.Configuration;

using Coolector.Common.RabbitMq;
using Coolector.Common.Security;

using servicedesk.SignalR.Hubs;
using servicedesk.SignalR.Services;
using servicedesk.Common.Services;
using servicedesk.Common.Commands;
using servicedesk.Common.Events;
using Coolector.Common.Extensions;

namespace servicedesk.SignalR
{
    public class Startup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string EnvironmentName { get; }
        public IConfiguration Configuration { get; }
        public static ILifetimeScope LifetimeScope { get; private set; }
        //TODO: get rid of static token handler after update signalr to newer version
        //public static IJwtTokenHandler JwtTokenHandler { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            EnvironmentName = env.EnvironmentName.ToLowerInvariant();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .SetBasePath(env.ContentRootPath);

            //if (env.IsProduction())
            //{
            //    builder.AddLockbox();
            //}

            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddRouting();

            services.AddCors(x => x.AddPolicy("corsGlobalPolicy", policy => {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            }));

            //services.AddSignalR();
            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });

            var assembly = Assembly.GetEntryAssembly();
            var builder = new ContainerBuilder();
            SecurityContainer.Register(builder, Configuration);
            RabbitMqContainer.Register(builder, Configuration.GetSettings<RawRabbitConfiguration>());
            builder.Populate(services);
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterType<Handler>().As<IHandler>();
            builder.RegisterType<StatusSignalRService>().As<IStatusSignalRService>();

            //builder.RegisterType<JwtTokenHandler>().As<IJwtTokenHandler>();

            LifetimeScope = builder.Build().BeginLifetimeScope();
            //TODO: get rid of static token handler after update signalr to newer version
            //JwtTokenHandler = LifetimeScope.Resolve<IJwtTokenHandler>();

            return new AutofacServiceProvider(LifetimeScope);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IApplicationLifetime appLifeTime)
        {
            loggerFactory.AddNLog();
            //loggerFactory.ConfigureNLog("nlog.config");

            app.UseCors("corsGlobalPolicy");

            app.UseWebSockets();
            app.UseSignalR("/hub");
            
            //app.UseSignalR<RawConnection>("/signalr");
            //app.UseSignalR();

            //app.UseSignalR("/hub", typeof(ServiceDeskHub));
            //app.UseSignalR(builder => builder.MapHub<ServiceDeskHub>("/hub"));

            appLifeTime.ApplicationStopped.Register(() => LifetimeScope.Dispose());
        }
    }
}
