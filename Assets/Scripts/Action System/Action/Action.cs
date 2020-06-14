
public class Action
{
    public ActionController ActionController { get; set; }
    public ActionParameters ActionParameters { get; set; }


    public void Execute()
    {
        ActionController.Execute(ActionParameters);
    }

    public bool HasFinshed()
    {
        return ActionController == null || ActionController.HasFinshed();
    }
}

