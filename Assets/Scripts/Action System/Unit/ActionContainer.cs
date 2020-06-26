using UnityEngine;

[System.Serializable]
public class ActionContainer
{
    public Action action;
    public ActionView view;
    public ActionController controller;


    public QueuedAction GetQueueAction(ActionTarget actionTarget)
    {
        return new QueuedAction { Action = action, ActionController = controller, ActionTarget = actionTarget };
    }

    public void ActiveView(Unit unit)
    {
        view.Activate(unit, action, controller);
    }

    public void UpdateView(Vector2 location)
    {
        view.UpdateView(location);
    }

    public void ClearView()
    {
        view.Clear();
    }
}
