using Action.Interfaces;
using Autofac;

namespace Action.Core
{
    public static class RegisterInjection
    {
        public static void RegisterActionCore(ContainerBuilder builder)
        {
            builder.RegisterType<ActionParser>().As<IActionParser>();
            builder.RegisterType<ActionExecutor>().As<IActionExecutor>();
            builder.RegisterType<ActionInfoProvider>().As<IActionInfoProvider>();
        }
    }
}
