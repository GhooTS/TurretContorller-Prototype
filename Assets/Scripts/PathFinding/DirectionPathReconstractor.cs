using System.Collections.Generic;
using UnityEngine;

namespace Nav2D
{

    /// <summary>
    /// Recostract path base on direction changing
    /// </summary>
    public class DirectionPathReconstractor : IPathReconstractor
    {
        /// <summary>
        /// Recostract path base on direction changing
        /// </summary>
        public List<Vector2> RecreatePath(Dictionary<Node, Node> cameFrom, Node start, Node goal)
        {
            var path = new List<Vector2>();
            var current = goal;
            Vector2 currentDirection = Vector2.zero;
            path.Add(current.GetNodeCenter());
            while ((Vector2Int)current.position != (Vector2Int)start.position)
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
                    Debug.LogError("Data for path reconstration was incorrect");
                    return null;
                }
            }

            path.Reverse();

            return path;
        }
    }
}