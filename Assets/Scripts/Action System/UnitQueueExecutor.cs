using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class UnitQueueExecutor : MonoBehaviour
{
    public UnityEvent CycleStarted;
    [Tooltip("Invoked after all entities performed all assigned action, and there is no reaction left to execute")]
    public UnityEvent CycleEnded;
    [Tooltip("Invoked after entity performed all assigned action ,and there is no reaction left to execute")]
    public UnityEvent EntityTourEnded;

    /// <summary>
    /// Currently active unit in cycle
    /// </summary>
    public Unit CurrentActive { get; private set; }
    public bool InCycle { get; private set; } = false;

    private readonly UnitQueue unitQueue = new UnitQueue();
    [SerializeField]
    private ReactionQueue reactionQueue;
    private List<Unit> entities;
    /// <summary>
    /// currently performed action in cycle, null means no action
    /// </summary>
    private QueuedAction currentAction;
    /// <summary>
    /// currently performed reaction in cycle, null means no reaction
    /// </summary>
    private Reaction currentReaction;

    private void Start()
    {
        entities = FindObjectsOfType<Unit>().ToList();
        foreach (var entity in entities)
        {
            entity.ResetActionPoint();
        }
    }


    private void Update()
    {
        if (InCycle)
        {
            MoveToNextUnitOrEndCycle();
        }
    }


    private void MoveToNextUnitOrEndCycle()
    {
        //Early return if action hasn't finished
        if (currentAction != null && currentAction.ActionController.HasFinshed() == false) return;

        //Early return if reaction hasn't finished
        if (currentReaction != null && currentReaction.Finshed() == false) return;

        if (reactionQueue.HasNext()) 
        {
            currentReaction = reactionQueue.Next();
            currentReaction.Start();
        }
        else if (CurrentActive != null && CurrentActive.HasNextAction())
        {
            reactionQueue.Reset(true);
            currentAction = CurrentActive.DequeueAction();
            currentAction.Execute();
        }
        else if (CurrentActive != null)
        {
            EntityTourEnded?.Invoke();

            if (unitQueue.HasNext())
            {
                CurrentActive = unitQueue.Next();
            }
            else
            {
                EndCycle();
            }
        }
    }

    /// <summary>
    /// Starts next cycle with all assign action in 
    /// </summary>
    public void StartNextCycle()
    {
        if (InCycle) return;

        
        unitQueue.CreateNewQueue(entities);
        
        CurrentActive = unitQueue.Next();
        InCycle = true;
        CycleStarted?.Invoke();
    }

    private void EndCycle()
    {
        InCycle = false;
        foreach (var entity in entities)
        {
            entity.ResetActionPoint();
        }

        CurrentActive = null;
        currentAction = null;
        currentReaction = null;
        CycleEnded?.Invoke();
    }

}