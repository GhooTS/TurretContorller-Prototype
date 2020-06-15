using UnityEngine;

namespace Nav2D
{
    [System.Serializable]
    public class Node
    {
        public enum NodeType
        {
            wall, free
        }

        public float cost = 1.0f;
        //TODO : Add better way to compare position without jump value (mayby splite position to position and jumpValue)
        public Vector3Int position;
        public NodeType type;

        public int x { get { return position.x; } }
        public int y { get { return position.y; } }
        public int JumpCost { get { return position.z; } set { position.z = value; } }

        public Node(Vector2Int position, NodeType type)
        {
            this.position = new Vector3Int(position.x, position.y, 0);
            this.type = type;
        }
        public Node(int x, int y, NodeType type)
        {
            position = new Vector3Int(x, y, 0);
            this.type = type;
        }

        public Node(int x, int y, int jumpValue, NodeType type)
        {
            position = new Vector3Int(x, y, jumpValue);
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

        public Node Clone(int jumpValue)
        {
            return new Node(x, y, jumpValue, type);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}