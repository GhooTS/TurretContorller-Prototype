using UnityEngine;

[System.Serializable]
public class MovementLimiter
{
    public Bounds bounds;
    [Range(0, 100)]
    public float margin;
    public Transform boundsCenterTarget;

    public Vector2 GetPosition(Vector2 movePosition)
    {
        SetBoundsCenterToTargetPosition();

        movePosition.x = Mathf.Clamp(movePosition.x, bounds.min.x - margin, bounds.max.x + margin);
        movePosition.y = Mathf.Clamp(movePosition.y, bounds.min.y - margin, bounds.max.y + margin);

        return movePosition;
    }

    public Vector2 GetPosition(Vector2 movePosition, Vector2 cameraSize)
    {
        SetBoundsCenterToTargetPosition();

        float xMin = bounds.min.x + cameraSize.x - margin;
        float xMax = bounds.max.x - cameraSize.x + margin;
        float yMin = bounds.min.y + cameraSize.y - margin;
        float yMax = bounds.max.y - cameraSize.y + margin;

        movePosition.x = Mathf.Clamp(movePosition.x, xMin, xMax);
        movePosition.y = Mathf.Clamp(movePosition.y, yMin, yMax);

        return movePosition;
    }

    private void SetBoundsCenterToTargetPosition()
    {
        if (boundsCenterTarget != null)
        {
            bounds.center = boundsCenterTarget.position;
        }
    }

    public void DrawGizmo(Color color)
    {
        SetBoundsCenterToTargetPosition();

        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.one * margin * 2);
    }
}
