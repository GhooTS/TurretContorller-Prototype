using UnityEngine;

[CreateAssetMenu(menuName = "Action System/View/Precise Shot")]
public class PreciseShotView : ActionView
{
    public LayerMask layerMask;
    public LineRenderer display;
    private LineRenderer instance;
    private ShootAction action;
    private ShootActionController controller;

    public override void Clear()
    {
        Destroy(instance.gameObject);
    }

    public override void Activate(Unit _, Action action,ActionController controller)
    {
        this.action = action as ShootAction;
        this.controller = controller as ShootActionController;
        instance = Instantiate(display);
        instance.positionCount = 2;
        instance.SetPosition(0,Vector2.zero);
        instance.SetPosition(1,Vector2.zero);
    }

    public override void UpdateView(Vector2 location)
    {
        var startPoint = (Vector2)controller.rotationPoint.position;
        var hit = Physics2D.Raycast(startPoint,(location - startPoint),action.range,layerMask);
        location = hit.collider == null ? startPoint + (location - startPoint).normalized * action.range : hit.point;

        instance.SetPosition(0, startPoint);
        instance.SetPosition(1, location);
    }
}
