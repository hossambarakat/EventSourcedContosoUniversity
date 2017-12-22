using System.IO;
using Akka.Actor;
using Microsoft.Extensions.Configuration;
using EventSourcedContosoUniversity.Core.ReadModel.Departments;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using Akka.DI.Core;
using Autofac;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel;
using EventSourcedContosoUniversity.Core.Infrastructure.IoC;

namespace EventSourcedContosoUniversity.ReadModel
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var container = BuildContainer();

            using (var system = ActorSystem.Create("projections"))
            {
                var resolver = new Akka.DI.AutoFac.AutoFacDependencyResolver(container, system);
                IActorRef departmentsProjectionActor = system.ActorOf(system.DI().Props<DepartmentsProjectionActor>(), "DepartmentsProjectionActor");
                system.WhenTerminated.Wait();
            }

        }

        private static IContainer BuildContainer()
        {
            var eventStoreSettings = Configuration.Get<EventStoreSettings>();
            var readModelSettings = Configuration.Get<ReadModelSettings>();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(eventStoreSettings);
            containerBuilder.RegisterInstance(readModelSettings);

            containerBuilder.RegisterType<DepartmentsProjectionActor>().AsSelf();
            containerBuilder.RegisterType<EventStoreDispatcher>().AsSelf();
            containerBuilder.RegisterType<CatchupPositionRepository>().AsImplementedInterfaces();

            containerBuilder.RegisterModule<DefaultModule>();
            var container = containerBuilder.Build();
            return container;
        }
    }
}
