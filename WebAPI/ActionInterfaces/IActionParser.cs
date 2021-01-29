using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IActionParser
    {
        IAction<T> CreateAction<T>(ActionDescription actionDescription);
    }
}

