using Autofac;
using Action.Core;
using Action.Interfaces;
using System.Linq;
using AuctionModule.Commands;
using System;
using Services;
using DataAccess;

namespace InjectionConfig
{
    public static class ConfigureInjection
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterDataAccess();
            builder.RegisterInjectedServices();
            builder.RegisterActionCore();

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
            var actions = types.Where(IsAction);

            foreach (var action in actions)
            {
                var name = action.Name;
                builder.RegisterType(action).Keyed(name, typeof(IAction));
            }
        }

        private static bool IsAction(Type type)
        {
            var interfaceType = typeof(IAction);
            return interfaceType.IsAssignableFrom(type) && !type.IsInterface;
        }
    }
}
