using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System.Linq;


public static class Heuristics
{
    public static float GetManhattan(Vector2 from,Vector2 to)
    {
        return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
    }
}

public class PathFinder
{
#if UNITY_EDITOR
    public List<Vector3> serachPoints;
#endif

    public bool goalFound = false;
    private IPathReconstractor pathReconstractor = new PathReconstractor();

    public bool TryGetPath(Node start,Node goal,GridMap map,out List<Vector2> path, float maxJumpCost = 0.0f)
    {

        if (start == null || goal == null || map == null)
        {
            path = null;
            return false;
        }

        var cameFrom = new Dictionary<Node, Node>(); //Dictionary use to recreate path
        var frontier = new SimplePriorityQueue<Node,float>();
        var nodeCost = new Dictionary<Node, float>();
        var nodeJumpCost = new Dictionary<Node, float>();
        

        frontier.Enqueue(start, 0);
        cameFrom.Add(start, null);
        nodeCost.Add(start, 0);
        nodeJumpCost.Add(start, 0); // Assume starting node for agent is walkable

        var jumpFactor = maxJumpCost <= 0.0f ? 1.0f : 1 / maxJumpCost;
        float moveDirection = 0f;

        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();

            //exit loop and reconstruct path if goal found
            if (current == goal)
            {
#if UNITY_EDITOR
                if (cameFrom != null)
                {
                    serachPoints = cameFrom.Values.ToList().ConvertAll(node => node == null ? Vector3.zero : (Vector3)node.GetNodeCenter());
                }
#endif
                path = pathReconstractor.RecreatePath(cameFrom, start, goal);
                return true;
            }

            
            nodeJumpCost.TryGetValue(current, out moveDirection);

            foreach (var neightbor in map.GetNeightbors(current,moveDirection))
            {
                var cost = nodeCost[current] + neightbor.cost;
                var notVisited = !nodeCost.ContainsKey(neightbor);

                if(notVisited || cost < nodeCost[neightbor])
                {
                    if (TryGetJumpCost(nodeJumpCost, current, neightbor, maxJumpCost, out float jumpCost) == false) continue;

                    if (notVisited)
                    {
                        nodeCost.Add(neightbor, cost);
                        cameFrom.Add(neightbor, current);
                        nodeJumpCost.Add(neightbor, jumpCost);
                    }
                    else
                    {
                        nodeCost[neightbor] = cost;
                        cameFrom[neightbor] = current;
                        nodeJumpCost[neightbor] = jumpCost;
                    }

                    var piority = Heuristics.GetManhattan(neightbor.position,goal.position) + cost;

                    frontier.Enqueue(neightbor, piority);
                }
            }

        }
#if UNITY_EDITOR
        if (cameFrom != null)
        {
            serachPoints = cameFrom.Values.ToList().ConvertAll(node => node == null ? Vector3.zero : (Vector3)node.GetNodeCenter());
        }
#endif
        path = null;
        return false;
    }

    //Utility function for TryGetPath
    private bool TryGetJumpCost(Dictionary<Node,float> nodeJumpCost,Node current,Node neightbor,float maxJumpCost,out float jumpCost)
    {
        var currentJumpCost = nodeJumpCost.TryGetValue(current, out float newJumpCost) ? newJumpCost : 0.0f;
        jumpCost = currentJumpCost + neightbor.position.y - current.position.y;

        //Reset jump cost if neightbor node is walkable
        if (neightbor.IsNodeOfType(Node.NodeType.walkable)) jumpCost = 0.0f;

        //Continue if jump cost is higher then maxJumpCost
        if (jumpCost > maxJumpCost) return false;

        //Contunue if jumping start point isn't walkable
        if (jumpCost > 0 && currentJumpCost == 0.0f && current.IsNodeOfType(Node.NodeType.walkable) == false) return false;


        return true;
    }

    public void DrawLastSearchLocation()
    {
#if UNITY_EDITOR
        if (serachPoints != null)
        {
            for (int i = 0; i < serachPoints.Count; i++)
            {
                var factor = (float)i / serachPoints.Count;
                Gizmos.color = Color.Lerp(Color.green, Color.red, factor);
                Gizmos.DrawCube(serachPoints[i], Vector3.one * Mathf.Lerp(0.3f, 0.6f, factor));
            }
        }
#endif
    }
}
