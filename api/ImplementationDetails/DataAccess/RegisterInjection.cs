using System.Collections.Concurrent;
using Autofac;
using DataAccess.Interfaces;
using Models.AuctionsModule;

namespace DataAccess
{
    public static class RegisterInjection
    {
        public static ContainerBuilder RegisterDataAccess(this ContainerBuilder builder)
        {
            RegisterForTests(builder);

            return builder;
        }

        private static void RegisterForTests(ContainerBuilder builder)
        {
            var database = new ConcurrentDictionary<int, Auction>();
            builder.RegisterInstance(database).As<ConcurrentDictionary<int, Auction>>();

            builder
            .RegisterType<InMemoryGenericRepository<Auction>>()
            .As<IGenericRepostitory<int, Auction>>();
        }
    }
}
