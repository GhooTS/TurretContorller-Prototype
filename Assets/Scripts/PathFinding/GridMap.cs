using System;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    private Node[,] nodes;
    public Vector2Int startPosition;
    public bool diagnonalMovement;
    private Bounds mapBounds;

    public bool IsAgentOnMap(Vector2 position)
    {
        return mapBounds.Contains(position);
    }
    public void SetMap(int width,int height,Vector2Int startPosition)
    {
        width = Mathf.Max(0, width);
        height = Mathf.Max(0, height);

        nodes = new Node[width, height];
        var boundsCenter = new Vector2(startPosition.x + width / 2, startPosition.y + height / 2);
        var boundsSize = new Vector2(width, height);
        mapBounds = new Bounds(boundsCenter, boundsSize);
        this.startPosition = startPosition;
    }

    public void SetNode(int x,int y,Node node)
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

    public Node.NodeType GetNodeType(int x,int y)
    {
        return nodes[x, y].type;
    }

    public void SetNodeType(int x,int y,Node.NodeType type)
    {
        nodes[x, y].type = type;
    }

    public List<Node> GetNeightbors(Node node, float verticalMoveDirection)
    {
        var output = new List<Node>();
        var IndexVector = GetNodeIndex(node);

        //Vertical neightbors
        if(node.IsNodeOfType(Node.NodeType.free) || node.IsNodeOfType(Node.NodeType.edge))
        {
            if (IsValideIndex(IndexVector.x, IndexVector.y + 1) && nodes[IndexVector.x, IndexVector.y + 1].type != Node.NodeType.wall)
                output.Add(nodes[IndexVector.x, IndexVector.y + 1]);

            if (IsValideIndex(IndexVector.x, IndexVector.y - 1) && nodes[IndexVector.x, IndexVector.y - 1].type != Node.NodeType.wall)
                output.Add(nodes[IndexVector.x, IndexVector.y - 1]);

            if (verticalMoveDirection > 0)
            {

                if (IsValideIndex(IndexVector.x + 1, IndexVector.y) && nodes[IndexVector.x + 1, IndexVector.y].type == Node.NodeType.walkable)
                    output.Add(nodes[IndexVector.x + 1, IndexVector.y]);

                if (IsValideIndex(IndexVector.x - 1, IndexVector.y) && nodes[IndexVector.x - 1, IndexVector.y].type == Node.NodeType.walkable)
                    output.Add(nodes[IndexVector.x - 1, IndexVector.y]);
            }
        }
        else
        {

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xIndex = IndexVector.x + x;
                    int yIndex = IndexVector.y + y;

                    if (diagnonalMovement == false && Mathf.Abs(x + y) != 1) continue;

                    if (IsValideIndex(xIndex, yIndex) && nodes[xIndex, yIndex].type != Node.NodeType.wall)
                    {
                        output.Add(nodes[xIndex, yIndex]);
                    }
                }
            }

        }


        

        return output;
    }

    private Vector2Int GetNodeIndex(Node node)
    {
        var output = node.position - startPosition;
        return output;
    }

    public Vector2Int FromPositionToIndex(Vector2 position)
    {
        return Vector2Int.FloorToInt(position - startPosition);
    }

    public Vector2 GetClosesPoint(Vector2 position)
    {
        return mapBounds.ClosestPoint(position);
    }

    public Node GetNode(Vector2 position)
    {
        return GetNode(FromPositionToIndex(position));
    }

    public Node GetNode(Vector2Int indexVector)
    {
        return GetNode(indexVector.x, indexVector.y);
    }

    public Node GetNode(int x,int y)
    {
        return IsValideIndex(x,y) ? nodes[x, y] : null;
    }

    public bool IsValideIndex(int x,int y)
    {
        return x >= 0 && x < nodes.GetLength(0) && y >= 0 && y < nodes.GetLength(1);
    }

    public void DrawNodesGizmos()
    {
        if (nodes == null) return;

        foreach (var node in nodes)
        {
            switch (node.type)
            {
                case Node.NodeType.wall:
                    Gizmos.color = Color.red;
                    continue;
                case Node.NodeType.free:
                    Gizmos.color = Color.yellow;
                    continue;
                case Node.NodeType.walkable:
                    Gizmos.color = Color.green;
                    break;
                case Node.NodeType.edge:
                    Gizmos.color = Color.cyan;
                    break;
                default:
                    Gizmos.color = Color.gray;
                    break;
            }
            Gizmos.DrawCube(node.GetNodeCenter(), Vector2.one * .2f);
        }
    }
}
