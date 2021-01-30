using System.Collections.Generic;
using System.ComponentModel;
using Autofac.Features.Indexed;
using Action.Interfaces;
using Action.Models;
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
            var actionWithParams = JsonConvert.DeserializeObject(json, actionType) as IAction;
            BindProperties(action, actionWithParams);

            return action;
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

        private void BindProperties(IAction actionFromServices, IAction actionFromJson)
        {
            var actionType = actionFromServices.GetType();
            var properties = actionType.GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(actionFromJson);
                property.SetValue(actionFromServices, propertyValue);
            }
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
