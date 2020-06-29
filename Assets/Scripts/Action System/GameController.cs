using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public UnitQueueExecutor unitQueueExecutor;
    public CameraController cameraController;

    private void Update()
    {
        if (unitQueueExecutor.InCycle)
        {
            if (unitQueueExecutor.CurrentActionFinshed() && unitQueueExecutor.CurrentReactionFinshed())
            {
                unitQueueExecutor.EndActionOrReaction();
                unitQueueExecutor.MoveToNext();
                if (unitQueueExecutor.CurrentAction != null || (unitQueueExecutor.CurrentReaction != null && unitQueueExecutor.CurrentReaction.RequierFocus))
                {
                    StartCameraTransition();
                }
                else
                {
                    unitQueueExecutor.StartNextActionOrReaction();
                }
            }
        }
    }

    private void StartCameraTransition()
    {
        StartCoroutine(WaitForCameraTransition());
    }

    private IEnumerator WaitForCameraTransition()
    {
        if (unitQueueExecutor.CurrentAction != null)
        {
            cameraController.StartTransition(unitQueueExecutor.CurrentActive.transform.position, unitQueueExecutor.CurrentAction.ActionTarget.targetLocation);
        }
        else if(unitQueueExecutor.CurrentReaction != null)
        {
            cameraController.StartTransition(unitQueueExecutor.CurrentReaction.Target);
        }
        yield return new WaitForSeconds(cameraController.transitionTime);
        unitQueueExecutor.StartNextActionOrReaction();
    }
}
