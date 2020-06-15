using UnityEngine;

namespace Nav2D
{
    public static class Heuristics
    {
        public static float GetManhattan(Vector2 from, Vector2 to, float D = 1f)
        {
            return D * (Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y));
        }
    }
}