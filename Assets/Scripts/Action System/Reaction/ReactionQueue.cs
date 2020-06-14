using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action System/Reaction", fileName = "newReactionQueue")]
public class ReactionQueue : ScriptableObject
{
    private readonly Queue<Reaction> reactions = new Queue<Reaction>();
    [SerializeField]
    private WaitReaction waitReaction;
    private bool queueEndedWithWaitReaction = true;

    public int Count { get { return reactions.Count; } }

    public bool HasNext()
    {
        return reactions.Count != 0 || queueEndedWithWaitReaction == false;
    }

    public Reaction Next()
    {
        if (reactions.Count != 0)
        {
            return reactions.Dequeue();
        }
        else if (queueEndedWithWaitReaction == false)
        {
            queueEndedWithWaitReaction = true;
            return waitReaction;
        }

        return null;
    }

    public void AddReaction(Reaction reaction)
    {
        reactions.Enqueue(reaction);
    }

    public void Reset(bool endQueueWithWaitTimeReaction = false)
    {
        reactions.Clear();
        queueEndedWithWaitReaction = endQueueWithWaitTimeReaction == false;
    }
}
