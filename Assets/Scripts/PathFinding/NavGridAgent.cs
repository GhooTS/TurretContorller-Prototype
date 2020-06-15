﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav2D
{
    public class NavGridAgent : MonoBehaviour
    {
        public float stopDistnace = 0f;
        public float moveSpeed = 10f;
        public float jumpSpeed = 15f;
        public float fallSpeed = 20f;
        public NavGridAgentModel agentModel;

        [Header("DEBUG")]
        public bool logToConsole = false;
        public bool drawPath = false;
        public bool drawSearchPosition = false;

        public NavGrid NavGrid { get; private set; }

        public bool HasPath { get; private set; } = false;
        public Vector2 MoveDirection { get { return pathTraveller.MoveDirection; } }

        private Pathfinder pathFinder = new Pathfinder();
        private PathTraveller pathTraveller = new PathTraveller();


        private void Start()
        {
            foreach (var navGrid in FindObjectsOfType<NavGrid>())
            {
                if (navGrid.IsWithinNavGridBounds(transform.position))
                {
                    NavGrid = navGrid;
                }
            }

            if (NavGrid == null)
            {
                enabled = false;
                Debug.LogError("There is not map present in agent location");
            }
        }


        private void Update()
        {
            if (HasPath)
            {
                transform.position = pathTraveller.GetNextPositon(transform.position, GetSpeed() * Time.deltaTime);

                if (pathTraveller.PathComplited)
                {
                    HasPath = false;
                    if (logToConsole)
                        Debug.Log("Destination reached");
                }
            }
        }

        private float GetSpeed()
        {
            return MoveDirection.y < 0 ? fallSpeed : MoveDirection.y > 0 ? jumpSpeed : moveSpeed;
        }

        public void SetPath(Path path)
        {
            pathTraveller.SetPath(path);
            HasPath = true;
        }

        public bool CalculatePath(Vector2 destination, out Path path)
        {
            if (NavGrid == null)
            {
                Debug.LogError($"Agent <i>{name}</i> is off GridMap", this);
                path = null;
                return false;
            }

            var start = NavGrid.GetNode(transform.position);
            var goal = NavGrid.GetNode(destination);

            if (goal != null && goal.IsNodeOfType(Node.NodeType.wall))
            {
                Debug.LogWarning($"Location for Agent <i>{name}</i> is unreachable", this);
                path = null;
                return false;
            }

            var timeNow = System.DateTime.Now;
            bool pathFound = pathFinder.TryGetPath(start, goal, NavGrid, agentModel, out List<Vector2> newPath);
            path = newPath != null ? new Path(newPath) : null;

            if (logToConsole)
            {
                Debug.Log($"Path {(pathFound ? "" : "not")} found for agent: <i>{name}</i> from {start.GetNodeCenter()} to {destination}"
                        + $" in time: {(System.DateTime.Now - timeNow).Duration().TotalMilliseconds} ms", this);
            }

            return pathFound;
        }

        public void SetDestination(Vector2 destination)
        {
            if (CalculatePath(destination, out Path path))
            {
                pathTraveller.SetPath(path);
                HasPath = true;
            }
        }

        private void OnDrawGizmosSelected()
        {

            if (drawSearchPosition)
            {
                pathFinder.DrawSerachTiles();
            }

            if (drawPath && pathTraveller != null)
            {
                var currentPath = pathTraveller.GetPath();
                if (currentPath == null) return;

                Gizmos.color = new Color(.25f, .7f, .25f);
                for (int i = 0; i < currentPath.Count - 1; i++)
                {
                    var from = currentPath[i];
                    var to = currentPath[i + 1];
                    Gizmos.DrawLine(from, to);
                    Gizmos.DrawCube(from, Vector2.one * .5f);
                    if (i == currentPath.Count - 2)
                    {
                        Gizmos.DrawCube(to, Vector2.one * .5f);
                    }
                }

            }

        }
    }
}