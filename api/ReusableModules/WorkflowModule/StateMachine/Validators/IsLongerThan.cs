using System;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine.Validators
{
    public class IsLongerThan : IInputValidator
    {
        public bool IsTrue(object[] parameters)
        {
            var leftParameter = parameters[0];
            var leftValueType = leftParameter.GetType();
            var rigthValue = Convert.ToInt32(parameters[1]);

            if (leftValueType.IsArray)
            {
                var testValue = leftParameter as dynamic;

                return testValue.Length > rigthValue;
            }

            var leftValue = parameters[0] as string ?? "";

            return leftValue.Length > rigthValue;
        }
    }
}
