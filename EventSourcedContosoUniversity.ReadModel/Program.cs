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
using Serilog;
using System;

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

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var container = BuildContainer();

            using (var system = ActorSystem.Create("projections", "akka { loglevel=DEBUG" + Environment.NewLine+
                "  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"] }"))
            {
                var resolver = new Akka.DI.AutoFac.AutoFacDependencyResolver(container, system);
                IActorRef departmentsProjectionActor = system.ActorOf(system.DI().Props<DepartmentsProjectionActor>(), "DepartmentsProjectionActor");
                IActorRef studentsProjectionActor = system.ActorOf(system.DI().Props<StudentsProjectionActor>(), "StudentsProjectionActor");
                IActorRef coursesProjectionActor = system.ActorOf(system.DI().Props<CoursesProjectionActor>(), "CoursesProjectionActor");
                Log.Logger.Information("Application Started");
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

            //TODO: automatic discover and register actors
            containerBuilder.RegisterType<DepartmentsProjectionActor>().AsSelf();
            containerBuilder.RegisterType<StudentsProjectionActor>().AsSelf();
            containerBuilder.RegisterType<CoursesProjectionActor>().AsSelf();

            containerBuilder.RegisterType<EventStoreDispatcher>().AsSelf();
            containerBuilder.RegisterType<CatchupPositionRepository>().AsImplementedInterfaces();

            containerBuilder.RegisterModule<DefaultModule>();
            var container = containerBuilder.Build();
            return container;
        }
    }
}
