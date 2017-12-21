using System.IO;
using Akka.Actor;
using Microsoft.Extensions.Configuration;
using EventSourcedContosoUniversity.Core.ReadModel.Departments;

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

            using (var system = ActorSystem.Create("projection"))
            {
                IActorRef myFirstActor = system.ActorOf(Props.Create<DepartmentsProjectionActor>(), "crap");
                system.WhenTerminated.Wait();
            }

        }
    }
}
