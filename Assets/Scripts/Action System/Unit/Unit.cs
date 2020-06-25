using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int speed = 1;
    [Min(0)]
    public int maxActionPoints = 1;
    protected int currentActionPoints;
    protected ActionQueue actionQueue = new ActionQueue();

    public bool EnqueueAction(Action action)
    {
        if (CanPerformedAction(action))
        {
            currentActionPoints -= action.GetCost();
            actionQueue.AddAction(action);
            return true;
        }

        return false;
    }

    public Action DequeueAction()
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

    public bool CanPerformedAction(Action action)
    {
        return currentActionPoints - action.GetCost() >= 0;
    }
}
