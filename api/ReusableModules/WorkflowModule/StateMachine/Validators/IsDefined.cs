using System.Linq;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine.Validators
{
    public class IsDefined : IInputValidator
    {
        public bool IsTrue(object[] parameters)
        {
            return parameters.All(p => p != null);
        }
    }
}
