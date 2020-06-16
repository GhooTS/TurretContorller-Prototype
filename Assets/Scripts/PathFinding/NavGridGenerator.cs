using UnityEngine;

namespace Nav2D
{
    public class NavGridGenerator : MonoBehaviour
    {
        public NavGrid navGrid;
        public Vector2Int offset;
        public float cellSize = 1;
        [Min(0)]
        public int width = 25;
        [Min(0)]
        public int height = 25;
        public LayerMask collisonLayer;
        [Range(0, 1)]
        public float precision = .9f;

        public bool showGrid = false;
        public bool showNavGrid = false;


        private void Awake()
        {
            GenerateMap();
        }

        [ContextMenu("Generate map")]
        public void GenerateMap()
        {
            navGrid.Init(width, height, offset);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var hit = Physics2D.BoxCast(navGrid.IndexToPosition(new Vector2Int(x,y)), Vector2.one * navGrid.CellSize * precision, 0, Vector2.zero, 0, collisonLayer);
                    var nodeType = hit.collider ? NavGrid.NodeType.wall : NavGrid.NodeType.free;

                    navGrid.SetNode(x, y, nodeType);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (showGrid && navGrid != null)
            {
                Gizmos.color = new Color(.7f, .7f, .2f, .5f);
                Vector2 halfCell = Vector2.one * cellSize / 2;
                Vector2 start;
                Vector2 end;
                for (int x = 0; x <= width; x++)
                {
                    start = navGrid.IndexToPosition(new Vector2Int(x, 0)) - halfCell;
                    end = navGrid.IndexToPosition(new Vector2Int(x, height)) - halfCell;
                    Gizmos.DrawLine(start,end);
                }

                for (int y = 0; y <= height; y++)
                {
                    start = navGrid.IndexToPosition(new Vector2Int(0, y)) - halfCell;
                    end = navGrid.IndexToPosition(new Vector2Int(width, y)) - halfCell;
                    Gizmos.DrawLine(start,end);
                }
            }
            if (showNavGrid)
            {
                navGrid.DrawNodesGizmos();
            }
        }
    }
}