
using System.Collections.Generic;

public class ActionQueue
{
    private readonly Queue<Action> actionQueue = new Queue<Action>();

    /// <summary>
    /// add action to queue
    /// </summary>
    /// <param name="action"></param>
    public void AddAction(Action action)
    {
        actionQueue.Enqueue(action);
    }

    /// <summary>
    /// return next action in queue
    /// </summary>
    /// <returns></returns>
    public Action Next()
    {
        return actionQueue.Dequeue();
    }

    public bool HasAction()
    {
        return actionQueue.Count != 0;
    }

    public bool RemoveLastAction()
    {
        return actionQueue.Dequeue() != null;
    }
}
