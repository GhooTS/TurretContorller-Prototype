//jump adaptation in the A * algorithm was obtained from the tutorial described by Daniel Branicki
//https://gamedevelopment.tutsplus.com/tutorials/how-to-adapt-a-pathfinding-to-a-2d-grid-based-platformer-implementation--cms-24679

using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace Nav2D
{
    public class Pathfinder
    {
        //TODO : Craete better path reconstractor
        private IPathReconstractor pathReconstractor = new DirectionPathReconstractor();
        //TODO : Create proper pathfinding debugger
        private PathfinderDebugger debugger = new PathfinderDebugger();
        private bool goalFound = false;

        private const int MINIMAL_DICTIONARY_SIZE = 50;

        public bool TryGetPath(Vector2 startPosition, Vector2 goalPosition, NavGrid navGrid, NavGridAgentModel agentModel, out List<Vector2> path)
        {

            if (navGrid == null || navGrid.IsWithinNavGridBounds(goalPosition) == false)
            {
                path = null;
                return false;
            }

            goalFound = false;

            var startIndexVector = navGrid.PositionToIndex(startPosition);
            var goalIndexVector = navGrid.PositionToIndex(goalPosition);

            if (navGrid.IsNodeOfType(goalIndexVector.x,goalIndexVector.y,NavGrid.NodeType.wall))
            {
                path = null;
                return false;
            }

            var predictSize = (int)Heuristics.GetManhattan(startPosition, goalPosition) * 2;
            predictSize = Mathf.Max(MINIMAL_DICTIONARY_SIZE, predictSize);

            var cameFrom = new Dictionary<Vector3Int, Node>(predictSize); //Dictionary use to recreate path
            var jumpNodeData = new Dictionary<Vector2Int, JumpData>(predictSize);
            var open = new SimplePriorityQueue<Node, float>();

            Node current = null;
            Node start = new Node(startIndexVector);

            open.Enqueue(start, 0);
            cameFrom.Add(new Vector3Int(start.x,start.y,0), null);

            while (open.Count != 0)
            {
                current = open.Dequeue();

                //early exit if goal found
                if (current.position == goalIndexVector)
                {
                    goalFound = true;
                    break;
                }

                bool jumpOpportunity = false;

                if (current.jumpCost == 0)
                {
                    var distanceToGoal = Heuristics.GetManhattan(current.position, goalIndexVector);
                    jumpOpportunity = current.y < goalIndexVector.y && distanceToGoal <= agentModel.maxJumpHeight * 2
                                      || HesJumpOpportunity(navGrid, current.x, current.y, agentModel.maxJumpHeight);
                }

                foreach (var neightbor in navGrid.GetNeightbors(current.position))
                {
                    var atCeling = false;
                    var grounded = false;

                    //continue if neightbor is the start node
                    if (neightbor == start.position) continue;

                    if (navGrid.IsGroundedNode(neightbor.x,neightbor.y))
                        grounded = true;
                    else if (navGrid.IsCelingNode(neightbor.x, neightbor.y))
                        atCeling = true;

                    var jumpCost = GetJumpCost(current, neightbor, atCeling, grounded, agentModel);

                    if (grounded == false)
                    {
                        //Prevent starting the jump if there is non location to jump to
                        if (current.jumpCost == 0 && jumpCost == 3 && jumpOpportunity == false) continue;
                        //Allow horizontal movement only from even nodes
                        if (current.jumpCost >= 0 && current.jumpCost % 2 != 0 && current.x != neightbor.x) continue;
                        //Prevent from moving up further than the agent max jump height allow
                        if (current.jumpCost >= agentModel.maxJumpHeight * 2 && current.y < neightbor.y) continue;
                        //After first 3 cells horizontal movement is allowed every 4-th cell (on cells with jumpvalue of 13,21,29...)
                        if (jumpCost >= agentModel.maxJumpHeight * 2 + 6 && neightbor.x != current.x
                            && (jumpCost - (agentModel.maxJumpHeight * 2 + 6)) % 8 != 3)
                            continue;
                    }


                    Vector3Int location = new Vector3Int(neightbor.x, neightbor.y, jumpCost);

                    //-1f means node was not visited
                    float oldCost = cameFrom.TryGetValue(location, out Node oldNode) ? oldNode.cost : -1f;
                    var cost = current.cost + 1 + jumpCost * 0.001f;

                    if (oldCost == -1f || oldCost > cost) 
                    {
                        if (jumpCost != 0)
                        {
                            var hesNodeJumpData = jumpNodeData.TryGetValue(neightbor, out JumpData jumpData);

                            if (hesNodeJumpData && jumpData.lowerJumpValue <= jumpCost)
                            {
                                if (jumpCost % 2 != 0 || jumpData.horizontalMovement || jumpCost >= agentModel.maxJumpHeight * 2 + 6)
                                {
                                    continue;
                                }
                            }

                            jumpData.lowerJumpValue = jumpCost;
                            jumpData.horizontalMovement |= jumpCost % 2 == 0;

                            if (hesNodeJumpData)
                            {
                                jumpNodeData[neightbor] = jumpData;
                            }
                            else
                            {
                                jumpNodeData.Add(neightbor, jumpData);
                            }
                        }

                        if (oldCost == -1)
                        {
                            cameFrom.Add(location, current);
                        }
                        else
                        {
                            cameFrom[location] = current;
                        }

                        Node newNode = new Node(neightbor)
                        {
                            cost = cost,
                            distance = Heuristics.GetManhattan(current.position, goalIndexVector),
                            jumpCost = jumpCost
                        };

                        open.Enqueue(newNode, newNode.Priority);
                    }
                }

            }

            //DEBUG-------------------------------
            debugger.SetSerachTiles(cameFrom,navGrid);
            //------------------------------------

            path = goalFound ? pathReconstractor.RecreatePath(cameFrom, start, current,navGrid) : null;
            return goalFound;
        }

        private int GetJumpCost(Node current, Vector2Int neightbor, bool atCeling, bool grounded, NavGridAgentModel agentModel)
        {
            var output = 0;

            if (atCeling)
            {
                if (current.x != neightbor.x)
                {
                    output = Mathf.Max(agentModel.maxJumpHeight * 2 + 1, current.jumpCost + 1);
                }
                else
                {
                    output = Mathf.Max(agentModel.maxJumpHeight * 2, current.jumpCost + 2);
                }
            }
            else if (grounded)
            {
                output = 0;
            }
            else if (current.y < neightbor.y) // jumping
            {
                if (current.jumpCost < 2)
                {
                    output = 3;
                }
                else
                {
                    output = current.jumpCost + (current.jumpCost % 2 == 0 ? 2 : 1);
                }
            }
            else if (current.y > neightbor.y) // falling
            {
                output = Mathf.Max(agentModel.maxJumpHeight * 2, current.jumpCost + (current.jumpCost % 2 == 0 ? 2 : 1));
            }
            else if (grounded == false && current.x != neightbor.x) // walling off the egde
            {
                output = current.jumpCost + 1;
            }

            return output;
        }

        //TODO : Addapt to diffrent agent size and height 
        private bool HesJumpOpportunity(NavGrid map, int x, int y, int maxJumpHeight)
        {
            //Check if agent is on the egde
            if (map.IsFreeNode(x - 1, y - 1) || map.IsFreeNode(x + 1, y - 1))
            {
                return true;
            }


            for (int i = 0; i < maxJumpHeight; i++)
            {

                if (map.IsFreeNode(x, y + i) == false || map.IsFreeNode(x, y + i + 1) == false)
                {
                    break;
                }

                if (map.IsFreeNode(x - 1, y + i) == false && map.IsFreeNode(x - 1, y + i + 1))
                {
                    return true;
                }

                if (map.IsFreeNode(x + 1, y + i) == false && map.IsFreeNode(x + 1, y + i + 1))
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
}