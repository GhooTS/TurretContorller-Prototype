using UnityEngine;
using GTVariable;

public class PreciseShotPreviewDisplay : ActionPreviewDisplay
{
    public LineRenderer display;
    [Header("Display Mode Colors")]
    public GradientReference normal;
    public GradientReference focus;
    public GradientReference active;


    public override void Init()
    {
        display.positionCount = 2;
        display.SetPosition(0, Vector2.zero);
        display.SetPosition(1, Vector2.zero);
    }

    public override void DestroyPreview()
    {
        Destroy(gameObject);
    }

    public override void SetPreviewMode(PreviewMode mode)
    {
        switch (mode)
        {
            case PreviewMode.Hidden:
                display.enabled = false;
                break;
            case PreviewMode.Visible:
                display.enabled = true;
                display.colorGradient = normal.Value;
                break;
            case PreviewMode.Focus:
                display.enabled = true;
                display.colorGradient = focus.Value;
                break;
            case PreviewMode.Active:
                display.enabled = true;
                display.colorGradient = active.Value;
                break;
            default:
                break;
        }

        Mode = mode;
    }

    public void SetDisplayTarget(Vector2 start,Vector2 target)
    {
        display.SetPosition(0, start);
        display.SetPosition(1, target);
    }
}
