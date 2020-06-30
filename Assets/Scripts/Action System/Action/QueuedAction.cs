
public class QueuedAction
{
    public ActionController ActionController { get; set; }
    public Action Action { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public ActionPreviewDisplay Display { get; set; }
    public bool HasStarted { get; private set; } = false;


    public void Execute()
    {
        Display.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Active);
        ActionController.Execute(Action, ActionTarget);
        HasStarted = true;
    }

    public bool HasFinshed()
    {
        if(ActionController == null || (ActionController.HasFinshed() && HasStarted)) 
        {
            Display.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Hidden);
            return true;
        }

        return false;
    }

    public int GetCost()
    {
        return Action.cost;
    }
}

