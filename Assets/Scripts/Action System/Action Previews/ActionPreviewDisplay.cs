using UnityEngine;

public abstract class ActionPreviewDisplay : MonoBehaviour
{
    public enum PreviewMode
    {
        Hidden,
        Visible,
        Focus,
        Active
    }
    public PreviewMode Mode { get; protected set; }
    public abstract void SetPreviewMode(PreviewMode mode);
    public abstract void DestroyPreview();
    public abstract void Init();
}
