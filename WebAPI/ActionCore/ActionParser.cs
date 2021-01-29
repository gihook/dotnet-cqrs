using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.ActionCore
{
    public class ActionParser : IActionParser
    {
        public IAction<T> CreateAction<T>(ActionDescription actionDescription)
        {
            return null;
        }
    }
}
