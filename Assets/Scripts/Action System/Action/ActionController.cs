using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// base class for all action controllers
/// </summary>
[System.Serializable]
public abstract class ActionController : MonoBehaviour
{
    [Min(0)]
    public int cost = 1;
    public bool active;

    public abstract void Execute(ActionParameters parameters);
    public abstract bool HasFinshed();
}

