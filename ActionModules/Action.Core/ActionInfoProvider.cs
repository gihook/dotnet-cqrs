using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.Indexed;
using Action.Interfaces;
using Action.Models;

namespace WebAPI.ActionCore
{
    public class ActionInfoProvider
    {
        private readonly IIndex<string, IAction> _serviceDictionary;
        private readonly ILifetimeScope _scope;

        public ActionInfoProvider(IIndex<string, IAction> serviceDictionary,
            ILifetimeScope scope)
        {
            _serviceDictionary = serviceDictionary;
            _scope = scope;
        }

        public IEnumerable<ActionInfo> AllActionInfos()
        {
            return _scope.ComponentRegistry.Registrations
         .Where(r => typeof(IAction).IsAssignableFrom(r.Activator.LimitType))
         .Select(r => r.Activator.LimitType.Name).Select(GetActionInfo);
        }

        public ActionInfo GetActionInfo(string actionName)
        {
            var action = _serviceDictionary[actionName];
            var actionType = action.GetType();
            var properties = actionType.GetProperties();
            var baseType = actionType.BaseType.Name;
            var fullName = actionType.FullName;

            var parameterInfos = properties.Select(GetParameterInfo);

            var actionInfo = new ActionInfo
            {
                Type = baseType,
                Name = actionName,
                FullName = fullName,
                Parameters = parameterInfos
            };

            return actionInfo;
        }

        private ActionParameter GetParameterInfo(PropertyInfo property)
        {
            var name = property.Name;
            var type = property.PropertyType.Name;
            var genericArguments = property.PropertyType.GenericTypeArguments.Select(t => t.Name);

            return new ActionParameter
            {
                Name = name,
                Type = type,
                Generics = genericArguments
            };
        }
    }
}
