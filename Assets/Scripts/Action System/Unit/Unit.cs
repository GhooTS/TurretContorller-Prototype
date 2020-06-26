using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum BelongTo
    {
        Player,
        AI
    }
    public int speed = 1;
    [Min(0)]
    public int maxActionPoints = 1;
    public BelongTo controlBy;
    protected int currentActionPoints;
    protected ActionQueue actionQueue = new ActionQueue();

    public bool EnqueueAction(QueuedAction action)
    {
        if (CanPerformeAction(action))
        {
            currentActionPoints -= action.GetCost();
            actionQueue.AddAction(action);
            return true;
        }

        return false;
    }

    public QueuedAction DequeueAction()
    {
        return actionQueue.Next();
    }

    public void ResetActionPoint()
    {
        currentActionPoints = maxActionPoints;
    }

    public bool HasNextAction()
    {
        return actionQueue.HasAction();
    }

    public bool HesActionPoints()
    {
        return currentActionPoints > 0;
    }

    public bool CanPerformeAction(QueuedAction action)
    {
        return currentActionPoints - action.GetCost() >= 0;
    }

    public bool CanPerformeAction(Action action)
    {
        return currentActionPoints - action.cost >= 0;
    }
}
