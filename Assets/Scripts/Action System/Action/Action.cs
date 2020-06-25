
public class Action
{
    public ActionController ActionController { get; set; }
    public ActionParameters ActionParameters { get; set; }
    public ActionTarget ActionTarget { get; set; }


    public void Execute()
    {
        ActionController.Execute(ActionParameters,ActionTarget);
    }

    public bool HasFinshed()
    {
        return ActionController == null || ActionController.HasFinshed();
    }

    public int GetCost()
    {
        return ActionParameters.cost;
    }
}

