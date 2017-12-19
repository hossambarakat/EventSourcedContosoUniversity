using Akka.Actor;

namespace EventSourcedContosoUniversity.Core
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
