using System;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine.Converters
{
    public class IntConverter : ITypeConverter
    {
        public bool CanParse(object data)
        {
            try
            {
                Convert.ToInt32(data);

                return true;
            }
            catch { return false; }
        }
    }
}
