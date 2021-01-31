using Action.Models;

namespace Action.Interfaces
{
    public interface IActionParser
    {
        IAction CreateAction(ActionDescription actionDescription);
        IAction ParseJson(string actionName, string json);
    }
}

