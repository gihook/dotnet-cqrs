using System.Collections.Concurrent;
using Autofac;
using Models.AuctionsModule;

namespace DataAccess
{
    public static class RegisterInjection
    {
        public static ContainerBuilder RegisterDataAccess(ContainerBuilder builder)
        {
            return builder;
        }

        private static void RegisterForTests(ContainerBuilder builder)
        {
            builder.RegisterType<ConcurrentDictionary<int, Auction>>();
        }
    }
}
