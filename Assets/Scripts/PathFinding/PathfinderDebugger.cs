using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Nav2D
{
    public class PathfinderDebugger
    {
#if UNITY_EDITOR
        private readonly List<Vector2> serachPoints = new List<Vector2>();
        private float cellSize = 1;
#endif

        public void SetSerachTiles(Dictionary<Vector3Int, Node> cameFrom,NavGrid navGrid)
        {
#if UNITY_EDITOR
            serachPoints.Clear();
            if (cameFrom != null)
            {
                serachPoints.AddRange(cameFrom.Values.ToList().ConvertAll(node => node == null ? default : navGrid.IndexToPosition(node.position)));
                cellSize = navGrid.CellSize;
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
                Gizmos.DrawCube(serachPoints[i], Vector2.one * cellSize / 2);
            }
#endif
        }

    }
}