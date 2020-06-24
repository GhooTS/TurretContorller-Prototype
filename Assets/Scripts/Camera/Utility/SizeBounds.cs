using UnityEngine;

public class SizeBounds : GameObjectBounds
{
    public Rect size;

    public override Bounds GetBounds()
    {
        Vector2 boundsCenter = transform.position + (Vector3)size.position;
        Bounds output = new Bounds(boundsCenter, size.size);

        return output;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)size.position, size.size);
    }
}
