namespace WorkflowModule.Models
{
    public class StateInfo
    {
        public string State { get; set; }
        public object StateData { get; set; }
        public int CurrentOrderNumber { get; set; }

        public static StateInfo NullState
        {
            get
            {
                var instance = new StateInfo();
                instance.State = "NULL_STATE";

                return instance;
            }
        }
    }
}
