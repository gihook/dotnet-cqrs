using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IActionParser
    {
        IAction CreateAction(ActionDescription actionDescription);
        IAction ParseJson(string actionName, string json);
    }
}

