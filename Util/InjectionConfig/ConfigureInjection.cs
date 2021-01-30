using Autofac;
using Action.Core;

namespace InjectionConfig
{
    public class ConfigureInjection
    {
        public static void Configure(ContainerBuilder builder)
        {
	     RegisterInjection.RegisterActionCore(builder);
        }
    }
}
