using System.Collections.Generic;
using UnityEngine;

namespace Nav2D
{

    public class NavGrid : MonoBehaviour
    {
        public enum NodeType
        {
            free, wall
        }

        [Tooltip("diagonal movement is not supported for 'platformer' pathfinding"
                + " enabling it in current state can lead to unknown behaviour")]
        public bool diagnonalMovement;

        /// <summary>
        /// Size of single cell in the navigation grid
        /// </summary>
        public float CellSize { get; private set; }
        public Vector2 Offset { get; private set; }
        private NodeType[,] nodes;
        private Bounds bounds;

        public bool IsWithinNavGridBounds(Vector2 position)
        {
            return bounds.Contains(position);
        }


        /// <summary>
        /// Initialize navigation grid with nodes of type <seealso cref="NodeType.free"/>,
        /// use this method to change navigation grid parameters as size, offset or cellSize.
        /// </summary>
        /// <param name="width">width of naviation grid</param>
        /// <param name="height">height of naviagtion grid</param>
        /// <param name="offset">naviagtion grid offset</param>
        /// <param name="cellSize">size of single cell in the navigation grid</param>
        public void Init(int width, int height, Vector2 offset, float cellSize = 1f)
        {
            width = Mathf.Max(0, width);
            height = Mathf.Max(0, height);

            nodes = new NodeType[width, height];
            Offset = offset;
            CellSize = cellSize;
            bounds = new Bounds(Vector3.zero, Vector3.zero);
            bounds.Encapsulate(IndexToPosition(new Vector2Int(0, 0)) - (Vector2.one * cellSize / 2));
            bounds.Encapsulate(IndexToPosition(new Vector2Int(width - 1, height - 1)) + (Vector2.one * cellSize / 2));
        }

        /// <summary>
        /// Sets node type for node with specify index
        /// </summary>
        /// <param name="nodeType">type of the node</param>
        public void SetNode(int x, int y, NodeType nodeType)
        {
            nodes[x, y] = nodeType;
        }

        /// <summary>
        /// Checks if node with index x, y is of specify type. Returns false if index excede range
        /// </summary>
        public bool IsNodeOfType(int x,int y,NodeType nodeType)
        {
            return IsValideIndex(x, y) && nodes[x, y] == nodeType;
        }


        /// <summary>
        /// return type of node with index x, y. Returns <seealso cref="NodeType.free"/> if index exceed range
        /// </summary>
        public NodeType GetNodeType(int x, int y)
        {
            return IsValideIndex(x, y) ? nodes[x, y] : NodeType.free;
        }

        /// <summary>
        /// Checks whether node is of type <seealso cref="NodeType.free"/>, returns true if index exceed range
        /// </summary>
        public bool IsFreeNode(int x, int y)
        {
            return GetNodeType(x,y) == NodeType.free;
        }

        /// <summary>
        /// Checks whether node is grounded or not, returns false if index exceed range
        /// </summary>
        public bool IsGroundedNode(int x,int y)
        {
            return GetNodeType(x, y - 1) == NodeType.wall;
        }

        /// <summary>
        /// Checks whether node is grounded or not, returns false if index exceed range
        /// </summary>
        public bool IsCelingNode(int x,int y)
        {
            return GetNodeType(x, y + 1) == NodeType.wall;
        }

        /// <summary>
        /// Get neighbors for specify node index
        /// </summary>
        /// <param name="indexVector">node index</param>
        /// <returns>list of neighbors indexes</returns>
        public List<Vector2Int> GetNeightbors(Vector2Int indexVector)
        {
            var output = new List<Vector2Int>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xIndex = indexVector.x + x;
                    int yIndex = indexVector.y + y;

                    if (diagnonalMovement == false && Mathf.Abs(x + y) != 1) continue;

                    if (IsNodeOfType(xIndex, yIndex, NodeType.free)) 
                    {
                        output.Add(new Vector2Int(xIndex, yIndex));
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Convert a node index to world position
        /// </summary>
        /// <param name="indexVector">node index</param>
        /// <returns>world position of node center</returns>
        /// <remarks>This method return center position of the node, whitch is determinate by <seealso cref="CellSize"/></remarks>
        public Vector2 IndexToPosition(Vector2Int indexVector)
        {
            var output = CellSize * (Vector2)indexVector + Vector2.one * (CellSize / 2) + Offset;
            
            return output;
        }

        /// <summary>
        /// Convert a world position to node index
        /// </summary>
        /// <param name="position">world position</param>
        /// <returns>node index</returns>
        public Vector2Int PositionToIndex(Vector2 position)
        {
            if (IsWithinNavGridBounds(position) == false) return Vector2Int.left;
            var distance = position - Offset;
            var vectorIndex = new Vector2Int((int)(distance.x / CellSize), (int)(distance.y / CellSize));
            vectorIndex.x = Mathf.Abs(vectorIndex.x);
            vectorIndex.y = Mathf.Abs(vectorIndex.y);
            return vectorIndex;
        }

        /// <summary>
        /// Returns closes point to specify position that is within navigation grid bounds
        /// </summary>
        public Vector2 GetClosesPoint(Vector2 position)
        {
            return bounds.ClosestPoint(position);
        }

        /// <summary>
        /// Checks if specified index does not exceed range
        /// </summary>
        public bool IsValideIndex(int x, int y)
        {
            return x >= 0 && x < nodes.GetLength(0) && y >= 0 && y < nodes.GetLength(1);
        }

        public void DrawNodesGizmos()
        {
            if (nodes == null) return;

            Gizmos.color = new Color(.3f, .3f, .7f);
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    if (IsFreeNode(x, y))
                    {
                        Gizmos.DrawSphere(IndexToPosition(new Vector2Int(x, y)), CellSize / 4f);
                    }
                }
            }
        }
    }
}