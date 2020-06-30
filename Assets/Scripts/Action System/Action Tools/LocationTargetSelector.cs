using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LocationTargetSelector : MonoBehaviour
{
    public UnityEvent activated;
    public UnityEvent targetSelected;
    public UnityEvent operationCanceled;
    private Unit actionSource;
    private ActionContainer actionContainer;
    private bool isActive = false;
    private Vector2 location;
    new private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }


    private void Update()
    {
        if (isActive == false) return;

        location = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        actionContainer.UpdatePreview(location);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Applay();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelAction();
        }
    }

    public void Applay()
    {
        var startPosition = actionContainer.controller.GetActionStartPosition(location);
        location = startPosition + (location - startPosition).normalized * actionContainer.action.range;
        actionSource.EnqueueAction(actionContainer.GetQueuedAction(new ActionTarget { targetLocation = location }));
        Deactivate();
        targetSelected?.Invoke();
    }

    public void Activate(Unit actionSource, ActionContainer actionContainer)
    {
        this.actionSource = actionSource;
        this.actionContainer = actionContainer;
        actionContainer.ActivePreview(actionSource);
        isActive = true;
        activated?.Invoke();
    }

    private void Deactivate()
    {
        actionSource = null;
        actionContainer = null;
        isActive = false;
    }

    private void CancelAction()
    {
        actionContainer.ClearPreview();
        Deactivate();
        operationCanceled?.Invoke();
    }

}
