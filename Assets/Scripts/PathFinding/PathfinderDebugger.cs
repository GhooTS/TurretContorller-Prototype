using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfinderDebugger
{
#if UNITY_EDITOR
    private readonly List<Vector2> serachPoints = new List<Vector2>();
#endif

    public void SetSerachTiles(Dictionary<Node,Node> cameFrom)
    {
#if UNITY_EDITOR
        serachPoints.Clear();
        if (cameFrom != null)
        {
            serachPoints.AddRange(cameFrom.Values.ToList().ConvertAll(node => node == null ? default : node.GetNodeCenter()));
        }
#endif
    }

    public void ShowLastSerachTiles()
    {
#if UNITY_EDITOR
        for (int i = 0; i < serachPoints.Count; i++)
        {
            var factor = (float)i / serachPoints.Count;
            Gizmos.color = Color.Lerp(Color.green, Color.red, factor);
            Gizmos.DrawCube(serachPoints[i],Vector2.one * .5f);
        }
#endif
    }

}
