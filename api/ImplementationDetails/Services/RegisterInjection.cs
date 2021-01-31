using Autofac;
using Models.AuctionsModule;
using Services.Interfaces;

namespace Services
{
    public static class RegisterInjection
    {
        public static ContainerBuilder RegisterInjectedServices(this ContainerBuilder builder)
        {
            builder
            .RegisterType<GenericService<int, Auction>>()
            .As<IGenericService<int, Auction>>();

            return builder;
        }
    }
}
