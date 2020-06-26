
using System.Collections.Generic;

public class ActionQueue
{
    private readonly Queue<QueuedAction> actionQueue = new Queue<QueuedAction>();

    /// <summary>
    /// add action to queue
    /// </summary>
    /// <param name="actionWrapper"></param>
    public void AddAction(QueuedAction actionWrapper)
    {
        actionQueue.Enqueue(actionWrapper);
    }

    /// <summary>
    /// return next action in queue
    /// </summary>
    /// <returns></returns>
    public QueuedAction Next()
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
