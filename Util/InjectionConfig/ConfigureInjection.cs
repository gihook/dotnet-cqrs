using Autofac;
using Action.Core;
using Action.Interfaces;
using System.Linq;
using AuctionModule.Commands;

namespace InjectionConfig
{
    public static class ConfigureInjection
    {
        public static void Configure(ContainerBuilder builder)
        {
            RegisterInjection.RegisterActionCore(builder);

            // NOTE: use ANY class from domain module
            // this is the only way to automatically register
            // Actions
            // It is not possible to reference DLL without
            // using at least one class in module
            RegisterAllActionsFromModule<CreateAuction>(builder);
        }

        private static void RegisterAllActionsFromModule<T>(ContainerBuilder builder)
        {
            var types = typeof(T).Assembly.GetTypes();
            var names = types.Select(t => t.Name);
            var actions = types;

            foreach (var action in actions)
            {
                var name = action.Name;
                System.Console.WriteLine("action: " + name);
                builder.RegisterType(action).Keyed(name, typeof(IAction));
            }
        }
    }
}
