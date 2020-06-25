using UnityEngine;

public abstract class ActionParameters : ScriptableObject
{
    [Min(0)]
    public int cost;
}

