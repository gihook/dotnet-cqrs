using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine.Converters
{
    public class ObjectConverter : ITypeConverter
    {
        public bool CanParse(object data)
        {
            return true;
        }
    }
}

