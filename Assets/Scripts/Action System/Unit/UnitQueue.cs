using Priority_Queue;
using System.Collections.Generic;

public class UnitQueue
{
    private readonly SimplePriorityQueue<Unit, int> unitsTurnQueue = new SimplePriorityQueue<Unit, int>();

    public int Count { get { return unitsTurnQueue.Count; } }

    public bool HasNext()
    {
        return unitsTurnQueue.Count != 0;
    }

    public Unit Next()
    {
        return unitsTurnQueue.Dequeue();
    }

    public void CreateNewQueue(List<Unit> units)
    {
        unitsTurnQueue.Clear();
        foreach (var unit in units)
        {
            unitsTurnQueue.Enqueue(unit, -unit.speed);
        }
    }

    public void CreateNewQueue(UnitsCollection units)
    {
        unitsTurnQueue.Clear();
        foreach (var unit in units)
        {
            unitsTurnQueue.Enqueue(unit, -unit.speed);
        }
    }


    public bool Remove(Unit entityController)
    {
        return unitsTurnQueue.TryRemove(entityController);
    }
}
