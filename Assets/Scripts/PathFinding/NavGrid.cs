using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

        public float CellSize { get; private set; }
        private NodeType[,] nodes;
        private Vector2Int offset;
        private Bounds bounds;

        public bool IsWithinNavGridBounds(Vector2 position)
        {
            return bounds.Contains(position);
        }

        public void Init(int width, int height, Vector2Int offset, float cellSize = 1f)
        {
            width = Mathf.Max(0, width);
            height = Mathf.Max(0, height);

            nodes = new NodeType[width, height];
            this.offset = offset;
            CellSize = cellSize;
            bounds = new Bounds(Vector3.zero, Vector3.zero);
            bounds.Encapsulate(IndexToPosition(new Vector2Int(0, 0)) - (Vector2.one * cellSize / 2));
            bounds.Encapsulate(IndexToPosition(new Vector2Int(width - 1, height - 1)) + (Vector2.one * cellSize / 2));
        }

        public void SetNode(int x, int y, NodeType node)
        {
            if (IsValideIndex(x, y))
            {
                nodes[x, y] = node;
            }
            else
            {
                Debug.LogError($"index {x}:{y} is invalide");
            }
        }

        public bool IsNodeOfType(int x,int y,NodeType nodeType)
        {
            return IsValideIndex(x, y) && nodes[x, y] == nodeType;
        }

        public NodeType GetNodeType(int x, int y)
        {
            return IsValideIndex(x, y) ? nodes[x, y] : NodeType.free;
        }

        public bool IsFreeNode(int x, int y)
        {
            return GetNodeType(x,y) == NodeType.free;
        }

        public bool IsGroundedNode(int x,int y)
        {
            return GetNodeType(x, y - 1) == NodeType.wall;
        }

        public bool IsCelingNode(int x,int y)
        {
            return GetNodeType(x, y + 1) == NodeType.wall;
        }

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


        public Vector2 IndexToPosition(Vector2Int indexVector)
        {
            var output = CellSize * (Vector2)indexVector + Vector2.one * (CellSize / 2) + offset;
            
            return output;
        }

        public Vector2Int PositionToIndex(Vector2 position)
        {
            if (IsWithinNavGridBounds(position) == false) return Vector2Int.left;
            var distance = position - offset;
            var vectorIndex = new Vector2Int((int)(distance.x / CellSize), (int)(distance.y / CellSize));
            vectorIndex.x = Mathf.Abs(vectorIndex.x);
            vectorIndex.y = Mathf.Abs(vectorIndex.y);
            return vectorIndex;
        }

        public Vector2 GetClosesPoint(Vector2 position)
        {
            return bounds.ClosestPoint(position);
        }

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