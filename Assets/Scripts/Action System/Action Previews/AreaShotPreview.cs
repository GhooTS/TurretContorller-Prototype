using UnityEngine;

[CreateAssetMenu(menuName = "Action System/View/AreaShot")]
public class AreaShotPreview : ActionPreview
{
    public LayerMask layerMask;
    public AreaShotPreviewDisplay display;
    private AreaShotPreviewDisplay instance;
    private ShootAction action;
    private ShootActionController controller;


    public override void Activate(Unit _, Action action, ActionController controller)
    {
        this.action = action as ShootAction;
        this.controller = controller as ShootActionController;
        instance = Instantiate(display,this.controller.rotationPoint.position,Quaternion.identity);
        instance.SetDisplayTarget(Vector2.right, this.controller.SpawnPointDistance, this.action.range + this.controller.SpawnPointDistance, this.action.bulletSpread);
        instance.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Focus);
    }

    public override void Clear()
    {
        Destroy(instance.gameObject);
    }

    public override void DetachPreview()
    {
        instance.SetPreviewMode(ActionPreviewDisplay.PreviewMode.Visible);
        instance = null;
    }

    public override ActionPreviewDisplay GetPreviewDisplay()
    {
        return instance;
    }

    public override void UpdateView(Vector2 location)
    {
        instance.SetDisplayTarget((location - (Vector2)controller.rotationPoint.position).normalized, controller.SpawnPointDistance, action.range + controller.SpawnPointDistance, action.bulletSpread);
    }
}
