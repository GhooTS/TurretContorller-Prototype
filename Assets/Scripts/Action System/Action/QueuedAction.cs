
public class QueuedAction
{
    public ActionController ActionController { get; set; }
    public Action Action { get; set; }
    public ActionTarget ActionTarget { get; set; }


    public void Execute()
    {
        ActionController.Execute(Action, ActionTarget);
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

