using UnityEngine;

[System.Serializable]
public class ActionContainer
{
    public Action action;
    public ActionPreview preview;
    public ActionController controller;


    public QueuedAction GetQueuedAction(ActionTarget actionTarget)
    {
        return new QueuedAction { Action = action, ActionController = controller, ActionTarget = actionTarget };
    }

    public void ActivePreview(Unit unit)
    {
        preview.Activate(unit, action, controller);
    }

    public void UpdatePreview(Vector2 location)
    {
        preview.UpdateView(location);
    }

    public void ClearPreview()
    {
        preview.Clear();
    }
}
