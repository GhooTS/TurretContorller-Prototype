using UnityEngine;
using UnityEngine.InputSystem;


//THIS CLASS REPRESENTS TEMPORARY SOLUTION
//AFTER ADDING THE UNIT SELECTION SYSTEM, THE CLASS WILL BE REFACTORED
public class ShootActionSelector : MonoBehaviour
{
    public LayerMask obstractionLayer;
    private Unit unit;
    private ShootActionController controller;
    private RangeShaderController display;
    private Vector2 target;
    private bool targetSelection = false;
    private float distanceToSpawnpoint = 0;
    private Selection selection;
    new private Camera camera;


    private void Start()
    {
        selection = FindObjectOfType<Selection>();
        camera = Camera.main;
    }

    private void Update()
    {

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ActiveTargetSelection();
        }

        if (targetSelection)
        {
            target = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 spawnPosition = controller.rotationPoint.position;
            Vector2 direction = (target - spawnPosition).normalized;
            display.SetArc(direction, distanceToSpawnpoint, controller.range, controller.bulletSpread);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                targetSelection = false;
                AddAction();
                selection.lockSelection = false;
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                targetSelection = false;
                selection.lockSelection = false;
                display.SetArc(Vector2.zero, 0, 0, 0);
            }

        }
    }
    public void AddAction()
    {
        var actionAdded = unit.EnqueueAction(new Action
        {
            ActionController = controller,
            ActionParameters = new ShootActionParameters { target = target }
        });
    }

    public void ActiveTargetSelection()
    {
        if (selection.HasSelection() && selection.selected.TryGetComponent(out unit) && selection.selected.TryGetComponent(out controller))
        {
            if (unit.HesActionPoints() == false) return;
            distanceToSpawnpoint = (controller.rotationPoint.position - controller.bulletSpawnPoint.position).magnitude;
            display = controller.display;
            targetSelection = true;
            selection.lockSelection = true;
        }
    }
}
