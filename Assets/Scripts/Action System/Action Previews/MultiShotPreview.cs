using UnityEngine;

[CreateAssetMenu(menuName = "Action System/View/MultiShot")]
public class MultiShotPreview : ActionPreview
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
        instance.SetDisplayTarget(Vector2.right, 0, this.action.range, this.action.bulletSpread);
    }

    public override void Clear()
    {
        Destroy(instance.gameObject);
    }

    public override void DetachPreview()
    {
        //Detach logic
    }

    public override ActionPreviewDisplay GetPreviewDisplay()
    {
        return instance;
    }

    public override void UpdateView(Vector2 location)
    {
        instance.SetDisplayTarget((location - (Vector2)controller.rotationPoint.position).normalized, 0, action.range, action.bulletSpread);
    }
}
