using System.Collections.Generic;
using System.Collections;

public class UnitsCollectionEnum : IEnumerator
{
    public readonly List<Unit> _units;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public UnitsCollectionEnum(List<Unit> list)
    {
        _units = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _units.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public Unit Current
    {
        get
        {
            try
            {
                return _units[position];
            }
            catch (System.IndexOutOfRangeException)
            {
                throw new System.InvalidOperationException();
            }
        }
    }
}
