using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UnitQueueExecutor : MonoBehaviour
{
    [SerializeField]
    private ReactionQueue reactionQueue;
    public UnityEvent CycleStarted;
    [Tooltip("Invoked after all entities performed all assigned action, and there is no reaction left to execute")]
    public UnityEvent CycleEnded;
    [Tooltip("Invoked after entity performed all assigned action ,and there is no reaction left to execute")]
    public UnityEvent EntityTurnEnded;
    public UnityEvent ActionEnded;
    public UnityEvent ReactionEnded;
    
    /// <summary>
    /// Currently active unit in cycle
    /// </summary>
    public Unit CurrentActive { get; private set; }
    public bool InCycle { get; private set; } = false;

    private readonly UnitQueue unitQueue = new UnitQueue();
    private List<Unit> entities;
    /// <summary>
    /// currently performed action in cycle, null means no action
    /// </summary>
    public QueuedAction CurrentAction { get; private set; }
    /// <summary>
    /// currently performed reaction in cycle, null means no reaction
    /// </summary>
    public Reaction CurrentReaction { get; private set; }



    private void Start()
    {
        entities = FindObjectsOfType<Unit>().ToList();
        foreach (var entity in entities)
        {
            entity.ResetActionPoint();
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
        CurrentAction = null;
        CurrentReaction = null;
        CycleEnded?.Invoke();
    }

    public void MoveToNext()
    {
        if (reactionQueue.HasNext())
        {
            CurrentReaction = reactionQueue.Next();
        }
        else if (CurrentActive != null && CurrentActive.HasNextAction())
        {
            reactionQueue.Reset(true);
            CurrentAction = CurrentActive.DequeueAction();
        }
        else if (CurrentActive != null)
        {
            EntityTurnEnded?.Invoke();

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

    public void StartNextActionOrReaction()
    {
        if (CurrentReaction != null)
        {
            CurrentReaction.Start();
        }
        else if (CurrentActive != null && CurrentAction != null)
        {
            CurrentAction.Execute();
        }
    }


    public void EndActionOrReaction()
    {
        if (CurrentActionFinshed() && CurrentAction != null)
        {
            CurrentAction = null;
            ActionEnded?.Invoke();
        }
        else if (CurrentReactionFinshed() && CurrentReaction != null)
        {
            CurrentReaction = null;
            ReactionEnded?.Invoke();
        }
    }


    public bool HasActionOrReaction()
    {
        return CurrentAction != null || CurrentReaction != null;
    }

    public bool CurrentActionFinshed()
    {
        return CurrentAction == null || CurrentAction.HasFinshed() && CurrentAction.HasStarted;
    }

    public bool CurrentReactionFinshed()
    {
        return CurrentReaction == null || CurrentReaction.Finshed();
    }

}