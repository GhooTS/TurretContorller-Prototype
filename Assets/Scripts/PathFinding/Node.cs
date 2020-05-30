using UnityEngine;

[System.Serializable]
public class Node
{
    public enum NodeType
    {
        wall,free,walkable,edge
    }

    public float cost = 1.0f;
    public Vector2Int position;
    public NodeType type;

    public Node(Vector2Int position,NodeType type)
    {
        this.position = position;
        this.type = type;
    }
    public Node(int x,int y,NodeType type)
    {
        this.position = new Vector2Int(x,y);
        this.type = type;
    }

    public Vector2 GetNodeCenter()
    {
        return new Vector2(position.x + 0.5f, position.y + 0.5f);
    }

    public bool IsNodeOfType(NodeType nodeType)
    {
        return type == nodeType;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
