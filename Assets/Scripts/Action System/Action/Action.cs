using System;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public enum ActionType
    {
        Shoot,
        Move
    }

    public enum TargetType
    {
        Unit,Location
    }

    [Flags]
    public enum TargetKind
    {
        Ally  = 1,
        Enemy = 2,
        Self  = 4
    }

    [Min(0)]
    public int cost;
    public string actionName;
    [TextArea]
    public string actionDescription;
    public Sprite icon;
    public bool targetable;
    public TargetType targetType;
    public TargetKind targetKind;
    public ActionType Type { get; protected set; }
    public float range;
}

