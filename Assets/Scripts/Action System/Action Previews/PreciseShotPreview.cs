using UnityEngine;

[CreateAssetMenu(menuName = "Action System/View/Precise Shot")]
public class PreciseShotPreview : ActionPreview
{
    public LayerMask layerMask;
    public PreciseShotPreviewDisplay display;
    private PreciseShotPreviewDisplay instance;
    private ShootAction action;
    private ShootActionController controller;

    public override void DetachPreview()
    {
        instance.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Visible);
        instance = null;
    }

    public override void Clear()
    {
        Destroy(instance.gameObject);
    }

    public override void Activate(Unit _, Action action,ActionController controller)
    {
        this.action = action as ShootAction;
        this.controller = controller as ShootActionController;
        instance = Instantiate(display);
        instance.Init();
        instance.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Focus);
    }

    public override void UpdateView(Vector2 location)
    {
        var startPoint = controller.GetActionStartPosition(location);
        var hit = Physics2D.Raycast(startPoint,(location - startPoint),action.range,layerMask);
        location = hit.collider == null ? startPoint + (location - startPoint).normalized * action.range : hit.point;

        instance.SetDisplayTarget(startPoint,location);
    }

    public override ActionPreviewDisplay GetPreviewDisplay()
    {
        return instance;
    }
}
