using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Selection : MonoBehaviour
{
    public Selectable selected;
    public UnityEvent OnSelected;
    public UnityEvent OnDiselected;
    public LayerMask selectionLayer;
    public bool lockSelection = false;
    new private Camera camera;


    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (lockSelection) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Select();
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Diselect();
        }
    }

    public void Select()
    {
        var mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (EventSystem.current.IsPointerOverGameObject()) return;

        var hit = Physics2D.Raycast(mousePosition, Vector2.zero,Mathf.Infinity,selectionLayer);
        if (hit.collider != null && hit.collider.TryGetComponent(out Selectable selected))
        {
            this.selected = selected;
            OnSelected?.Invoke();
        }
    }

    public void Diselect()
    {
        if (selected != null)
        {
            selected = null;
            OnDiselected?.Invoke();
        }
    }

    public bool HasSelection()
    {
        return selected != null;
    }
}
