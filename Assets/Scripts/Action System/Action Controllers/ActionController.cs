using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// base class for all action controllers
/// </summary>
[System.Serializable]
public abstract class ActionController : MonoBehaviour
{
    public bool active;

    public abstract void Execute(Action action,ActionTarget actionTarget);
    public abstract bool HasFinshed();
}

