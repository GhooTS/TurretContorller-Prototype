using UnityEngine;

public class SpriteBounds : GameObjectBounds
{
    public SpriteRenderer[] spriteRenderers;

    public override Bounds GetBounds()
    {
        if (spriteRenderers.Length == 0)
            return new Bounds(transform.position, Vector3.zero);

        Bounds output = spriteRenderers[0].bounds;

        for (int i = 1; i < spriteRenderers.Length; i++)
        {
            output.Encapsulate(spriteRenderers[i].bounds);
        }

        return output;
    }
}