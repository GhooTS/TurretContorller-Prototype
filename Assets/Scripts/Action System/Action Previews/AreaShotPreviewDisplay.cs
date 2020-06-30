using UnityEngine;
using GTVariable;

public class AreaShotPreviewDisplay : ActionPreviewDisplay
{
    public RangeShaderController display;
    [Header("Display Mode Colors")]
    public ColorReference normal;
    public ColorReference focus;
    public ColorReference active;


    public override void Init()
    {

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
                display.circleColor = normal.Value;
                break;
            case PreviewMode.Focus:
                display.enabled = true;
                display.circleColor = focus.Value;
                break;
            case PreviewMode.Active:
                display.enabled = true;
                display.circleColor = active.Value;
                break;
            default:
                break;
        }

        Mode = mode;
    }

    public void SetDisplayTarget(Vector2 direction, float innerRadius, float outterRadius, float halfArc)
    {
        display.SetArc(direction, innerRadius, outterRadius, halfArc);
    }
}
