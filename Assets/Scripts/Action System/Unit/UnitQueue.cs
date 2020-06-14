using Priority_Queue;
using System.Collections.Generic;

public class UnitQueue
{
    private readonly SimplePriorityQueue<Unit, int> entityTourQueue = new SimplePriorityQueue<Unit, int>();

    public int Count { get { return entityTourQueue.Count; } }

    public bool HasNext()
    {
        return entityTourQueue.Count != 0;
    }

    public Unit Next()
    {
        return entityTourQueue.Dequeue();
    }

    public void CreateNewQueue(List<Unit> enitities)
    {
        entityTourQueue.Clear();
        foreach (var entity in enitities)
        {
            entityTourQueue.Enqueue(entity, -entity.speed);
        }
    }

    public bool Remove(Unit entityController)
    {
        return entityTourQueue.TryRemove(entityController);
    }
}
