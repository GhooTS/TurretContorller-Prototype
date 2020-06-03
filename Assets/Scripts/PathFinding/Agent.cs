using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float stopDistnace = 0.001f;
    public float moveSpeed = 10f;
    public float jumpSpeed = 15f;
    public float fallSpeed = 20f;
    public float maxJumpHeight = 5.0f;

    public GridMap Map { get; private set; }

    public bool HasPath { get; private set; } = false;
    public Vector2 MoveDirection { get { return pathTraveller.MoveDirection; } }

    private PathFinder pathFinder = new PathFinder();
    private PathTraveller pathTraveller = new PathTraveller(); 


    private void Start()
    {
        foreach (var map in FindObjectsOfType<GridMap>())
        {
            if (map.IsAgentOnMap(transform.position))
            {
                Map = map;
            }
            Debug.Log(map);
        }

        if(Map == null)
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
        pathTraveller.Set(path);
        HasPath = true;
    }

    public bool CalculatePath(Vector2 destination,out Path path)
    {
        if (Map == null)
        {
            path = null;
            return false;
        }

        var start = Map.GetNode(transform.position);
        var goal = Map.GetNode(destination);

        //Check if the goal is valide
        if (goal != null)
        {
            if (pathFinder.TryGetPath(start, goal, Map, out List<Vector2> newPath, maxJumpHeight))
            {
                Debug.Log($"Path found for agent: <i>{name}</i> from {start.GetNodeCenter()} to {goal.GetNodeCenter()}");
                path = new Path(newPath);
                return true;
            }
        }
        else
        {
            Debug.Log($"Path not found for agent: <i>{name}</i> from {start.GetNodeCenter()} to {goal}");
        }
        path = null;
        return false;
    }

    public void SetDestination(Vector2 destination)
    {
        if (CalculatePath(destination, out Path path))
        {
            pathTraveller.Set(path);
            HasPath = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (pathTraveller == null) return;

        var currentPath = pathTraveller.Get();
        if (currentPath == null) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            var from = currentPath[i];
            var to = currentPath[i + 1];
            Gizmos.DrawLine(from, to);
            Gizmos.DrawCube(to, Vector2.one * 0.1f);
        }

        pathFinder.DrawLastSearchLocation();
    }
}