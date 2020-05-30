using System.Collections.Generic;
using UnityEngine;

public class PathReconstractor : IPathReconstractor
{
    public List<Vector2> RecreatePath(Dictionary<Node, Node> cameFrom, Node start, Node goal)
    {
        var path = new List<Vector2>();
        var current = goal;
        Vector2 currentDirection = Vector2.zero;
        path.Add(current.GetNodeCenter());
        while (current != start)
        {
            var currentPosition = current.GetNodeCenter();
            if (cameFrom.TryGetValue(current, out current))
            {
                var nodeCenterPosition = current.GetNodeCenter();
                var newDirection = nodeCenterPosition - currentPosition;
                if (currentDirection != newDirection)
                {
                    path.Add(nodeCenterPosition);
                    currentDirection = newDirection;
                }
                else
                {
                    path[path.Count - 1] = nodeCenterPosition;
                }
            }
            else
            {
                break;
            }
        }

        path.Reverse();

        return path;
    }
}
