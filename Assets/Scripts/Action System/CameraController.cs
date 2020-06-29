using UnityEngine;
using GTCamera2D;

public class CameraController : MonoBehaviour
{
    new public Camera camera;
    public CameraFreeLook freeCamera;
    public CameraMultiTargetFollow actionCamera;
    public float transitionTime = 1f;
    private SizeBounds actionSource;
    private SizeBounds target;

    private void Awake()
    {
        actionSource = new GameObject("Action Source", typeof(SizeBounds)).GetComponent<SizeBounds>();
        actionSource.size = new Rect(0f, 0f, 2f, 2f);
        target = new GameObject("Target", typeof(SizeBounds)).GetComponent<SizeBounds>();
        target.size = new Rect(0f, 0f, 2f, 2f);
    }

    public void SwitchToFreeCamera()
    {
        freeCamera.enabled = true;
        actionCamera.enabled = false;
    }

    public void SwitchToActionCamera()
    {
        freeCamera.enabled = false;
        actionCamera.enabled = true;
    }

    public void StartTransition(Vector2 source,Vector2 targetLocation)
    {
        actionCamera.ClearAllTargets();
        actionSource.transform.position = source;
        actionCamera.AddTarget(actionSource);
        target.transform.position = targetLocation;
        actionCamera.AddTarget(target);
    }

    public void StartTransition(Vector2 source)
    {
        actionCamera.ClearAllTargets();
        actionSource.transform.position = source;
        actionCamera.AddTarget(actionSource);
    }

    public void StartTransition(GameObjectBounds source, Vector2 targetLocation)
    {
        actionCamera.ClearAllTargets();
        actionCamera.AddTarget(source);
        target.transform.position = targetLocation;
        actionCamera.AddTarget(target);
    }

    public void StartTransition(GameObjectBounds source, GameObjectBounds target)
    {
        actionCamera.ClearAllTargets();
        actionCamera.AddTarget(source);
        actionCamera.AddTarget(target);
    }

    public void StartTransition(Transform source)
    {
        actionCamera.ClearAllTargets();
        actionCamera.AddTarget(source.GetComponent<GameObjectBounds>());
    }
}
