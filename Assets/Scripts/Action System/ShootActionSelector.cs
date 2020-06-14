using UnityEngine;
using UnityEngine.InputSystem;


//THIS CLASS REPRESENTS TEMPORARY SOLUTION
//AFTER ADDING THE UNIT SELECTION SYSTEM, THE CLASS WILL BE REFACTORED
public class ShootActionSelector : MonoBehaviour
{
    public Unit unit;
    private ShootActionController controller;
    public LineRenderer bulletTrajectoryDisplay;
    private Vector2 target;
    private bool targetSelection = false;
    private float distanceToSpawnpoint = 0;
    new private Camera camera;


    private void Start()
    {
        controller = unit.GetComponent<ShootActionController>();
        distanceToSpawnpoint = (controller.rotationPoint.position - controller.bulletSpawnPoint.position).magnitude;
        camera = Camera.main;
    }

    private void Update()
    {
        if (targetSelection)
        {
            target = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 spawnPosition = controller.rotationPoint.position;
            Vector2 hitPosition = Physics2D.Raycast(spawnPosition, target - (Vector2)spawnPosition).point;
            Vector2 direction = (hitPosition - spawnPosition).normalized;
            spawnPosition += direction * distanceToSpawnpoint;
            bulletTrajectoryDisplay.SetPosition(0,spawnPosition);
            bulletTrajectoryDisplay.SetPosition(1, hitPosition);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                targetSelection = false;
                AddAction();
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
        bulletTrajectoryDisplay.positionCount = 2;
        targetSelection = true;
    }
}
