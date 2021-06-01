using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine.Converters
{
    public class StringConverter : ITypeConverter
    {
        public bool CanParse(object data)
        {
            return true;
        }
    }
}
