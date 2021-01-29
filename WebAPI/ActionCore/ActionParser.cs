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
            System.Console.WriteLine($"Action: {actionDescription.ActionName}");
            var action = _serviceDictionary[actionDescription.ActionName];
            System.Console.WriteLine($"parsed: {action}");

            return action;
        }
    }
}
