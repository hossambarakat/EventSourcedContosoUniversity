using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using EventStore.ClientAPI;
using System.Net;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFeatureFolders();

            services.AddMediatR();

            var eventStoreConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            eventStoreConnection.ConnectAsync().Wait();
            services.AddSingleton(eventStoreConnection);
            services.AddScoped(typeof(IRepository<>), typeof(EventStoreRepository<>));

            services.Configure<ReadModelSettings>(Configuration);

            var readModelSettings = ServiceProvider.GetService<IOptions<ReadModelSettings>>();
            services.AddSingleton<IMongoClient>(new MongoClient(Configuration["MongoConnectionString"]));
            services.AddScoped<IReadModelRepository, MongoRepository>();
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
