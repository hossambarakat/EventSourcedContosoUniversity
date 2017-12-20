using Akka.Actor;
using EventSourcedContosoUniversity.Core.ReadModel.Departments;

namespace EventSourcedContosoUniversity.ReadModel
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (var system = ActorSystem.Create("projection"))
            {
                IActorRef myFirstActor = system.ActorOf(Props.Create<DepartmentsProjectionActor>(), "crap");
                system.WhenTerminated.Wait();
            }

        }
    }
}
