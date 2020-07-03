using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public UnitQueueExecutor unitQueue;
    public CameraController cameraController;
    public Animator GameStateAnimator;
    private int inCycleHash;

    private void Start()
    {
        inCycleHash = Animator.StringToHash("InCycle");
    }

    private void OnEnable()
    {
        
    }

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


    public void SwitchGameState()
    {
        GameStateAnimator.SetBool(inCycleHash,unitQueue.InCycle);
        Debug.Log(unitQueue.InCycle);
    }

    private void StartCameraTransition()
    {
        StartCoroutine(WaitForCameraTransition());
    }

    private IEnumerator WaitForCameraTransition()
    {
        if (unitQueue.HasAction())
        {
            cameraController.StartTransition(unitQueue.CurrentActive.transform, unitQueue.GetCurrentTarget());
        }
        else if(unitQueue.HasReaction())
        {
            cameraController.StartTransition(unitQueue.GetCurrentSource());
        }
        yield return new WaitForSeconds(cameraController.transitionTime);
        unitQueue.StartNextActionOrReaction();
    }
}
