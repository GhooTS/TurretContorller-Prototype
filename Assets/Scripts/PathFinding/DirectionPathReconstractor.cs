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
        public List<Vector2> RecreatePath(Dictionary<Vector3Int, Node> cameFrom, Node start, Node goal,NavGrid navGrid)
        {
            var path = new List<Vector2>();
            var current = goal;
            Vector2 currentDirection = Vector2.zero;
            path.Add(navGrid.IndexToPosition(current.position));
            while (current.position != start.position)
            {
                var currentPosition = navGrid.IndexToPosition(current.position);
                if (cameFrom.TryGetValue(new Vector3Int(current.x,current.y,current.jumpCost), out current))
                {
                    var nodeCenterPosition = navGrid.IndexToPosition(current.position);
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