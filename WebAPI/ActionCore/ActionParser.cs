using System.ComponentModel;
using Autofac.Features.Indexed;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.ActionCore
{
    public class ActionParser : IActionParser
    {
        private readonly IIndex<string, IAction> _serviceDictionary;

        public ActionParser(IIndex<string, IAction> serviceDictionary)
        {
            _serviceDictionary = serviceDictionary;
        }

        public IAction CreateAction(ActionDescription actionDescription)
        {
            var action = _serviceDictionary[actionDescription.ActionName];
            var actionType = action.GetType();

            foreach (var parameter in actionDescription.Parameters)
            {
                var propertyName = UppercaseFirst(parameter.Key);
                System.Console.WriteLine($"propertyName: {propertyName}");
                var property = actionType.GetProperty(propertyName);

                var propertyType = property.PropertyType;
                System.Console.WriteLine($"property: {propertyType}");

                var converter = TypeDescriptor.GetConverter(propertyType);
                System.Console.WriteLine($"converter: {converter}");

                var convertedValue = converter.ConvertFrom(parameter.Value);
                System.Console.WriteLine($"value: {convertedValue}");

                property.SetValue(action, convertedValue);
            }

            return action;
        }

        private string UppercaseFirst(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
