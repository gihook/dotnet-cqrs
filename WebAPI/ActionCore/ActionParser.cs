using System.Collections.Generic;
using System.ComponentModel;
using Autofac.Features.Indexed;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;
using Newtonsoft.Json;

namespace WebAPI.ActionCore
{
    public class ActionParser : IActionParser
    {
        private readonly IIndex<string, IAction> _serviceDictionary;

        public ActionParser(IIndex<string, IAction> serviceDictionary)
        {
            _serviceDictionary = serviceDictionary;
        }

        public IAction ParseJson(string actionName, string json)
        {
            var action = _serviceDictionary[actionName];
            var actionType = action.GetType();
            var actionWithParams = JsonConvert.DeserializeObject(json, actionType);
            System.Console.WriteLine(actionWithParams);

            return actionWithParams as IAction;
        }

        public IAction CreateAction(ActionDescription actionDescription)
        {
            var action = _serviceDictionary[actionDescription.ActionName];

            foreach (var parameter in actionDescription.Parameters)
            {
                SetConvertedValue(action, parameter);
            }

            return action;
        }

        private void SetConvertedValue(IAction action, KeyValuePair<string, object> parameter)
        {
            var actionType = action.GetType();
            var propertyName = UppercaseFirst(parameter.Key);
            var property = actionType.GetProperty(propertyName);
            var propertyType = property.PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            var convertedValue = converter.ConvertFrom(parameter.Value);

            property.SetValue(action, convertedValue);
        }

        private string UppercaseFirst(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
