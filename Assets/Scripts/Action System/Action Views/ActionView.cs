using UnityEngine;

public abstract class ActionView : ScriptableObject
{

    public abstract void Activate(Unit unit,Action action,ActionController controller);

    public abstract void UpdateView(Vector2 location);
    /// <summary>
    /// Clear view
    /// </summary>
    public abstract void Clear();
}
