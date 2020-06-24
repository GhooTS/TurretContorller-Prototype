using UnityEngine;
using UnityEngine.InputSystem;


//THIS CLASS REPRESENTS TEMPORARY SOLUTION
//AFTER ADDING THE UNIT SELECTION SYSTEM, THE CLASS WILL BE REFACTORED
public class ShootActionSelector : MonoBehaviour
{
    public LayerMask obstractionLayer;
    private Unit unit;
    private ShootActionController controller;
    public LineRenderer bulletTrajectoryDisplay;
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
            Vector2 hitPosition;
            var hit = Physics2D.Raycast(spawnPosition, target - spawnPosition, 20f, obstractionLayer);
            hitPosition = hit.collider == null ? spawnPosition + (target - spawnPosition).normalized * 20f  : hit.point;
            Vector2 direction = (hitPosition - spawnPosition).normalized;
            spawnPosition += direction * distanceToSpawnpoint;
            bulletTrajectoryDisplay.SetPosition(0,spawnPosition);
            bulletTrajectoryDisplay.SetPosition(1, hitPosition);

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

        if (actionAdded == false)
        {
            bulletTrajectoryDisplay.positionCount = 0;
            return;
        }

        var displayInstance = Instantiate(bulletTrajectoryDisplay);

        for (int i = 0; i < displayInstance.colorGradient.alphaKeys.Length; i++)
        {
            displayInstance.colorGradient.alphaKeys[i].alpha -= 0.1f;
        }
        bulletTrajectoryDisplay.positionCount = 0;
    }

    public void ActiveTargetSelection()
    {
        if (selection.HasSelection() && selection.selected.TryGetComponent(out unit) && selection.selected.TryGetComponent(out controller))
        {
            bulletTrajectoryDisplay.positionCount = 2;
            distanceToSpawnpoint = (controller.rotationPoint.position - controller.bulletSpawnPoint.position).magnitude;
            targetSelection = true;
            selection.lockSelection = true;
        }
    }
}
