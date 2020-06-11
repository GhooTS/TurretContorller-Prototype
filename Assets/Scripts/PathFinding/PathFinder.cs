//jump adaptation in the A * algorithm was obtained from the tutorial described by Daniel Branicki
//https://gamedevelopment.tutsplus.com/tutorials/how-to-adapt-a-pathfinding-to-a-2d-grid-based-platformer-implementation--cms-24679

using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Pathfinder
{
    //TODO : Craete better path reconstractor
    private IPathReconstractor pathReconstractor = new DirectionPathReconstractor();
    private PathfinderDebugger debugger = new PathfinderDebugger();
    private bool goalFound = false;

    private const int MINIMAL_DICTIONARY_SIZE = 50;

    public bool TryGetPath(Node start,Node goal,GridMap map,AgentModel agentModel,out List<Vector2> path)
    {
        
        if (start == null || goal == null || map == null)
        {
            path = null;
            return false;
        }

        goalFound = false;

        //TODO : Create proper pathfinding debugger
        //DEBUG VAR---------------------------
        //var counter = 0;
        //var createdNodesCounter = 0;
        //var queueMaxSize = 0;
        //------------------------------------

        var predictSize = (int)Heuristics.GetManhattan(start.GetNodeCenter(), goal.GetNodeCenter()) * 2;
        predictSize = Mathf.Max(MINIMAL_DICTIONARY_SIZE, predictSize);

        var cameFrom = new Dictionary<Node, Node>(Mathf.Max(predictSize)); //Dictionary use to recreate path
        var open = new SimplePriorityQueue<Node,float>();
        var jumpNodeData = new Dictionary<Vector2Int, JumpData>(predictSize);

        Node current = null;

        open.Enqueue(start, 0);
        cameFrom.Add(start, null);
        start.cost = 0;

        while (open.Count != 0)
        {
            //DEBUG
            //if(queueMaxSize < open.Count)
            //{
            //    queueMaxSize = open.Count;
            //}

            current = open.Dequeue();
            
            //early exit if goal found
            if ((Vector2Int)current.position == (Vector2Int)goal.position)
            {
                goalFound = true;
                break;
            }

            bool jumpOpportunity = false;

            if (current.JumpCost == 0) 
            {
                var distanceToGoal = Heuristics.GetManhattan(current.GetNodeCenter(), goal.GetNodeCenter());
                jumpOpportunity = (current.y < goal.y && distanceToGoal <= agentModel.maxJumpHeight * 2) 
                                  || HesJumpOpportunity(map, current.x, current.y, agentModel.maxJumpHeight);
            }

            foreach (var neightbor in map.GetNeightbors(current))
            {
                var atCeling = false;
                var grounded = false;

                if (map.GetNodeType(neightbor.x, neightbor.y - 1) == Node.NodeType.wall)
                    grounded = true;
                else if (map.GetNodeType(neightbor.x, neightbor.y + 1) == Node.NodeType.wall)
                    atCeling = true;

                var jumpValue = GetJumpValue(current,neightbor,atCeling,grounded,agentModel);
                //var cost = current.cost + 1;
                var cost = current.cost + 1 + (jumpValue * 0.001f);

                if (grounded == false)
                {
                    //Prevent starting the jump if there is non location to jump to
                    if (current.JumpCost == 0 && jumpValue == 3 && jumpOpportunity == false) continue;
                    //Allow horizontal movement only from even nodes
                    if (current.JumpCost >= 0 && current.JumpCost % 2 != 0 && current.x != neightbor.x) continue;
                    //Prevent from moving up further than the agent max jump height allow
                    if (current.JumpCost >= agentModel.maxJumpHeight * 2 && current.y < neightbor.y) continue;
                    //After first 3 cells horizontal movement is allowed every 4-th cell (on cells with jumpvalue of 13,21,29...)
                    if (jumpValue >= agentModel.maxJumpHeight * 2 + 6 && neightbor.x != current.x
                        && (jumpValue - (agentModel.maxJumpHeight * 2 + 6)) % 8 != 3)
                        continue;
                }

                //Create Clone for jump nodes 
                var newNeightbor = grounded ? neightbor : neightbor.Clone(jumpValue);

                var notVisited = !cameFrom.ContainsKey(newNeightbor);

                if (notVisited || cost < neightbor.cost)
                {

                    //Add jump data for jump nodes
                    if (jumpValue != 0)
                    {
                        var hesNodeJumpData = jumpNodeData.TryGetValue((Vector2Int)neightbor.position, out JumpData jumpData);

                        if (hesNodeJumpData && jumpData.lowerJumpValue <= jumpValue)
                        {
                            if (jumpValue % 2 != 0 || jumpData.horizontalMovement || jumpValue >= agentModel.maxJumpHeight * 2 + 6)
                            {
                                continue;
                            }
                        }

                        //DEBUG
                        //createdNodesCounter++;

                        jumpData.lowerJumpValue = jumpValue;
                        jumpData.horizontalMovement |= jumpValue % 2 == 0;

                        if (hesNodeJumpData)
                        {
                            jumpNodeData[(Vector2Int)neightbor.position] = jumpData;
                        }
                        else
                        {
                            jumpNodeData.Add((Vector2Int)neightbor.position, jumpData);
                        }
                    }

                    if (notVisited)
                    {
                        newNeightbor.cost = cost;
                        cameFrom.Add(newNeightbor, current);
                    }
                    else
                    {
                        newNeightbor.cost = cost;
                        cameFrom[newNeightbor] = current;
                    }

                    //TODO : Add support for more heuristics, (mayby by delegate?)
                    var piority = Heuristics.GetManhattan(neightbor.GetNodeCenter(), goal.GetNodeCenter()) + cost;
                    open.Enqueue(newNeightbor, piority);
                }

                //DEBUG
                //counter++;
            }

        }

        //DEBUG-------------------------------
        //Debug.Log($"Nodes serachted: {counter} || Nodes created: {createdNodesCounter} " +
        //          $"\n Queue max size: {queueMaxSize} || Predicted size: { predictSize} || Dictionary size {cameFrom.Count}");
        debugger.SetSerachTiles(cameFrom);
        //------------------------------------

        path = goalFound ? pathReconstractor.RecreatePath(cameFrom, start, current) : null;
        return goalFound;
    }

    private int GetJumpValue(Node current,Node neightbor, bool atCeling,bool grounded,AgentModel agentModel)
    {
        var output = 0;

        if (atCeling)
        {
            if (current.x != neightbor.x)
            {
                output = Mathf.Max(agentModel.maxJumpHeight * 2 + 1, current.JumpCost + 1);
            }
            else
            {
                output = Mathf.Max(agentModel.maxJumpHeight * 2, current.JumpCost + 2);
            }
        }
        else if (grounded)
        {
            output = 0;
        }
        else if (current.y < neightbor.y) // jumping
        {
            if (current.JumpCost < 2)
            {
                output = 3;
            }
            else
            {
                output = current.JumpCost + (current.JumpCost % 2 == 0 ? 2 : 1);
            }
        }
        else if (current.y > neightbor.y) // falling
        {
            output = Mathf.Max(agentModel.maxJumpHeight * 2, current.JumpCost + (current.JumpCost % 2 == 0 ? 2 : 1));
        }
        else if(grounded == false && current.x != neightbor.x) // walling off the egde
        {
            output = current.JumpCost + 1;
        }

        return output;
    }

    //TODO : Addapt to diffrent agent size and height 
    private bool HesJumpOpportunity(GridMap map,int x,int y,int agentHeight)
    {
        //Check if agent is on the egde
        if (map.IsFreeNode(x - 1, y - 1) || map.IsFreeNode(x + 1, y - 1))
        {
            return true;
        }


        for (int i = 0; i < agentHeight; i++)
        {

            if (map.IsFreeNode(x, y + i) == false
                || map.IsFreeNode(x, y + i + 1) == false)
            {
                break;
            }

            if (map.IsFreeNode(x - 1, y + i) == false && map.IsFreeNode(x - 1, y + i + 1))
            {
                return true;
            }

            if (map.IsFreeNode(x + 1, y + i) == false && map.IsFreeNode(x + 1, y + i + 1) )
            {
                return true;
            }
        }

        return false;
    }
    public void DrawSerachTiles()
    {
        debugger.ShowLastSerachTiles();
    }
}
