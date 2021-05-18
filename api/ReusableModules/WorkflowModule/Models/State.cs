namespace WorkflowModule.Models
{
    public class State
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsEqual(State state)
        {
            return Name == state.Name;
        }
    }
}

