
public class QueuedAction
{
    public ActionController ActionController { get; set; }
    public Action Action { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public bool HasStarted { get; private set; } = false;


    public void Execute()
    {
        ActionController.Execute(Action, ActionTarget);
        HasStarted = true;
    }

    public bool HasFinshed()
    {
        return ActionController == null || ActionController.HasFinshed();
    }

    public int GetCost()
    {
        return Action.cost;
    }
}

