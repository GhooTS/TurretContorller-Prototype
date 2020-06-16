using UnityEngine;

namespace Nav2D
{
    [System.Serializable]
    public class Node
    {
        public Vector2Int position;
        public int jumpCost;
        public float cost = 0;
        public float distance = 0;

        public int x { get { return position.x; } }
        public int y { get { return position.y; } }
        public float Priority { get { return cost + distance; } }

        public Node(Vector2Int position)
        {
            this.position = new Vector2Int(position.x, position.y);
        }
        public Node(int x, int y)
        {
            position = new Vector2Int(x, y);
        }

        public Node(int x, int y, int jumpCost)
        {
            position = new Vector2Int(x, y);
            this.jumpCost = jumpCost;
        }
    }
}