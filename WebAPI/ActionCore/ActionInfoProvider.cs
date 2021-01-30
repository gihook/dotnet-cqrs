using System.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.ActionCore
{
    public class ActionInfoProvider
    {
        private readonly IIndex<string, IAction> _serviceDictionary;

        public ActionInfoProvider(IIndex<string, IAction> serviceDictionary)
        {
            _serviceDictionary = serviceDictionary;
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
