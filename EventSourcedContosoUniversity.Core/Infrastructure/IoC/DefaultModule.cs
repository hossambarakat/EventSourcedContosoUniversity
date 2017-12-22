using Autofac;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.Infrastructure.IoC
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EventStoreRepository<>)).As(typeof(IRepository<>));

            builder.RegisterType<MongoRepository>().As<IReadModelRepository>().InstancePerLifetimeScope();

            builder.RegisterType<EventStoreConnectionFactory>().AsSelf().InstancePerLifetimeScope();

            builder.Register((c) =>
            {
                return c.Resolve<EventStoreConnectionFactory>().Create();
            });

            builder.Register((c) =>
            {
                return new MongoClient(c.Resolve<ReadModelSettings>().MongoConnectionString);
            }).AsImplementedInterfaces();
        }
    }
}
