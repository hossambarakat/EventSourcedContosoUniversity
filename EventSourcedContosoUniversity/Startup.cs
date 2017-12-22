using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using EventSourcedContosoUniversity.Core.ReadModel;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventSourcedContosoUniversity.Core.Infrastructure.IoC;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using FluentValidation.AspNetCore;

namespace EventSourcedContosoUniversity
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            ServiceProvider = serviceProvider;
        }
           
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFeatureFolders()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddOptions();

            services.AddMediatR();

            IContainer container = BuildContainer(services);
            return new AutofacServiceProvider(container);
        }

        private IContainer BuildContainer(IServiceCollection services)
        {
            var eventStoreSettings = Configuration.Get<EventStoreSettings>();
            var readModelSettings = Configuration.Get<ReadModelSettings>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            containerBuilder.RegisterInstance(eventStoreSettings);
            containerBuilder.RegisterInstance(readModelSettings);

            containerBuilder.RegisterModule<DefaultModule>();
            var container = containerBuilder.Build();
            return container;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
