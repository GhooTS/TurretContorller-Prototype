using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "Action System/Units Collection")]
public class UnitsCollection : ScriptableObject,IEnumerable
{
    private readonly List<Unit> units = new List<Unit>();

    public int Count { get { return units.Count; } }

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Reset()
    {
        units.Clear();
    }

    public void Add(Unit unit)
    {
        if (units.Contains(unit) == false)
        {
            units.Add(unit);
        }
    }

    public void Remove(Unit unit)
    {
        units.Remove(unit);
    }

    public Unit this[int i]
    {
        get { return units[i]; }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator)GetEnumerator();
    }

    public UnitsCollectionEnum GetEnumerator()
    {
        return new UnitsCollectionEnum(units);
    }
}
