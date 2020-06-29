using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public UnitQueueExecutor unitQueue;
    public CameraController cameraController;

    private void Update()
    {
        if (unitQueue.InCycle)
        {
            if (unitQueue.CurrentActionFinshed() && unitQueue.CurrentReactionFinshed())
            {
                unitQueue.EndActionOrReaction();
                unitQueue.MoveToNext();
                if (unitQueue.HasAction()|| (unitQueue.HasReaction() && unitQueue.CurrentReaction.RequierFocus))
                {
                    StartCameraTransition();
                }
                else
                {
                    unitQueue.StartNextActionOrReaction();
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
        if (unitQueue.HasAction())
        {
            cameraController.StartTransition(unitQueue.GetCurrentSource(), unitQueue.GetCurrentTarget());
        }
        else if(unitQueue.HasReaction())
        {
            cameraController.StartTransition(unitQueue.GetCurrentSource());
        }
        yield return new WaitForSeconds(cameraController.transitionTime);
        unitQueue.StartNextActionOrReaction();
    }
}
