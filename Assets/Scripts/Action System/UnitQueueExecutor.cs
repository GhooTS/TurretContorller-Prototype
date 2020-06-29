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
    /// currently performe action in cycle, null means no action
    /// </summary>
    public QueuedAction CurrentAction { get; private set; }
    /// <summary>
    /// currently performe reaction in cycle, null means no reaction
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
        if (HasReaction())
        {
            CurrentReaction.Start();
        }
        else if (CurrentActive != null && HasAction())
        {
            CurrentAction.Execute();
        }
    }


    public void EndActionOrReaction()
    {
        if (CurrentActionFinshed() && HasAction())
        {
            CurrentAction = null;
            ActionEnded?.Invoke();
        }
        else if (CurrentReactionFinshed() && HasReaction())
        {
            CurrentReaction = null;
            ReactionEnded?.Invoke();
        }
    }


    public bool HasActionOrReaction()
    {
        return HasAction() || HasReaction();
    }

    public bool CurrentActionFinshed()
    {
        return HasAction() == false || CurrentAction.HasFinshed() && CurrentAction.HasStarted;
    }

    public bool CurrentReactionFinshed()
    {
        return HasReaction() == false || CurrentReaction.Finshed();
    }

    public bool HasAction()
    {
        return CurrentAction != null;
    }

    public bool HasReaction()
    {
        return CurrentReaction != null;
    }

    public Vector2 GetCurrentSource()
    {
        return HasAction() ? (Vector2)CurrentActive.transform.position : (HasReaction() && CurrentReaction.RequierFocus ? CurrentReaction.Target : Vector2.zero);
    }

    public Vector2 GetCurrentTarget()
    {
        return HasAction() ? CurrentAction.ActionTarget.targetLocation : Vector2.zero;
    }

}