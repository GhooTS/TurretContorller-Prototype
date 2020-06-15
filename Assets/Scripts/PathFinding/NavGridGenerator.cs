using UnityEngine;
using UnityEngine.Tilemaps;

namespace Nav2D
{
    public class NavGridGenerator : MonoBehaviour
    {
        public NavGrid navGrid;
        public Vector2Int startPoint;
        [Min(0)]
        public int width;
        [Min(0)]
        public int height;
        public LayerMask collisonLayer;
        [Range(0, 1)]
        public float precision = 1;

        public bool showScanGrid = false;
        public bool showNavGrid = false;


        private void Awake()
        {
            GenerateMap();
        }

        [ContextMenu("Generate map")]
        public void GenerateMap()
        {
            navGrid.Init(width, height, startPoint);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var currentPosition = startPoint + new Vector2(x + 0.5f, y + 0.5f);

                    var hit = Physics2D.BoxCast(currentPosition, Vector2.one * precision, 0, Vector2.zero, 0, collisonLayer);
                    var nodeType = hit.collider ? Node.NodeType.wall : Node.NodeType.free;
                    var xPosition = navGrid.startPosition.x + x;
                    var yPosition = navGrid.startPosition.y + y;

                    navGrid.SetNode(x, y, new Node(xPosition, yPosition, nodeType));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (showScanGrid)
            {
                Gizmos.color = new Color(.7f, .7f, .2f, .5f);
                for (int x = 0; x <= width; x++)
                {
                    var start = startPoint + Vector2.right * x;
                    var end = startPoint + new Vector2(x, height);
                    Gizmos.DrawLine(start, end);
                }

                for (int y = 0; y <= height; y++)
                {
                    var start = startPoint + Vector2.up * y;
                    var end = startPoint + new Vector2(width, y);
                    Gizmos.DrawLine(start, end);
                }
            }
            if (showNavGrid)
            {
                navGrid.DrawNodesGizmos();
            }
        }
    }
}