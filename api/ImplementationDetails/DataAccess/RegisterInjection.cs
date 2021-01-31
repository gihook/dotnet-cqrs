using System.Collections.Concurrent;
using Autofac;
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
            builder.RegisterType<ConcurrentDictionary<int, Auction>>();
            builder
            .RegisterType<InMemoryGenericRepository<int, Auction>>()
            .As<InMemoryGenericRepository<int, Auction>>();
        }
    }
}
