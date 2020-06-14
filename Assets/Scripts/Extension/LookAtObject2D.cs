using UnityEngine;

public static class LookAtObject2D
{


    public enum FacingDirection
    {
        up, down, right, left
    };

    public static void LookAt2D(this Transform transform, Transform target, FacingDirection facingDirection = FacingDirection.right)
    {
        transform.LookAt2D(target.position, facingDirection);
    }

    public static void LookAt2D(this Transform transform, Vector3 target, FacingDirection facingDirection = FacingDirection.right)
    {
        transform.rotation = transform.GetLookAtRotation(target, facingDirection);
    }


    public static Quaternion GetLookAtRotation(this Transform transform, Transform target, FacingDirection facingDirection = FacingDirection.right)
    {
        return transform.GetLookAtRotation(target.position, facingDirection);
    }

    public static Quaternion GetLookAtRotation(this Transform transform, Vector3 target, FacingDirection facingDirection = FacingDirection.right)
    {
        return Quaternion.AngleAxis(transform.GetLookAtAngle(target, facingDirection), transform.forward);
    }

    public static float GetLookAtAngle(this Transform transform, Transform target, FacingDirection facingDirection = FacingDirection.right)
    {
        return transform.GetLookAtAngle(target.position, facingDirection);
    }

    public static float GetLookAtAngle(this Transform transform, Vector3 target, FacingDirection facingDirection = FacingDirection.right)
    {
        Vector2 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = OffsetFacingDirection(facingDirection, angle);
        return angle;
    }

    private static float OffsetFacingDirection(FacingDirection facingDirection, float angle)
    {
        switch (facingDirection)
        {
            case FacingDirection.right:
                break;
            case FacingDirection.up:
                angle -= 90;
                break;
            case FacingDirection.left:
                angle -= 180;
                break;
            case FacingDirection.down:
                angle -= 270;
                break;
        }

        return angle;
    }
}
