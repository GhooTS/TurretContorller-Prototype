using UnityEngine;

public class GridMapGenerator : MonoBehaviour
{
    public GridMap map;
    public Vector2Int startPoint;
    [Min(0)]
    public int width;
    [Min(0)]
    public int height;
    public LayerMask collisonLayer;
    [Range(0,1)]
    public float precision = 1;

    public bool showGrid = false;
    public bool showMap = false;


    private void Awake()
    {
        GenerateMap();
    }

    [ContextMenu("Generate map")]
    public void GenerateMap()
    {
        map.SetMap(width, height, startPoint);


        //Generate Map first phase
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var currentPosition = startPoint + new Vector2(x + 0.5f, y + 0.5f);

                var hit = Physics2D.BoxCast(currentPosition, Vector2.one * precision, 0, Vector2.zero, 0, collisonLayer);
                var nodeType = hit.collider ? Node.NodeType.wall : Node.NodeType.free;
                var xPosition = map.startPosition.x + x;
                var yPosition = map.startPosition.y + y;

                map.SetNode(x,y,new Node(xPosition,yPosition,nodeType));
            }
        }

        //Generate Map secound phase
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map.GetNodeType(x, y) != Node.NodeType.free) continue;

                if(IsWalkableNode(x,y))
                {
                    map.SetNodeType(x, y, Node.NodeType.walkable);
                }
            }
        }

        //Generate Map thrid phase
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map.GetNodeType(x, y) != Node.NodeType.free) continue;

                if (IsEdgeNode(x, y))
                {
                    map.SetNodeType(x, y, Node.NodeType.edge);
                }
            }
        }

        
    }

    private bool IsWalkableNode(int x,int y)
    {
        return y == 0 || map.GetNodeType(x, y - 1) == Node.NodeType.wall;
    }

    private bool IsEdgeNode(int x, int y)
    {
        var sideNodeFree = false;
        if (map.IsValideIndex(x - 1, y))
        {
            sideNodeFree = IsNodeOfType(x - 1, y, Node.NodeType.walkable);
        }
        if (map.IsValideIndex(x + 1, y))
        {
            sideNodeFree = sideNodeFree || IsNodeOfType(x + 1, y, Node.NodeType.walkable);
        }

        var diagonalNodesWalkable = false;

        if (map.IsValideIndex(x - 1, y - 1))
        {
            diagonalNodesWalkable = IsNodeOfType(x - 1, y - 1, Node.NodeType.wall);
        }
        if (map.IsValideIndex(x + 1, y - 1))
        {
            diagonalNodesWalkable = diagonalNodesWalkable || IsNodeOfType(x + 1, y - 1, Node.NodeType.wall);
        }

        return sideNodeFree && diagonalNodesWalkable && !IsNodeOfType(x,y-1,Node.NodeType.wall);
    }

    private bool IsNodeOfType(int x,int y,Node.NodeType type)
    {
        return map.GetNodeType(x, y) == type;
    }

    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireCube(startPoint + new Vector2(x + .5f, y + .5f), Vector2.one);
                }
            }
        }
        if (showMap)
        {
            map.DrawNodesGizmos();
        }
    }
}
