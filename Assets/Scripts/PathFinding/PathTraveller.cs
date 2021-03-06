﻿using UnityEngine;

namespace Nav2D
{
    public class PathTraveller
    {
        private Path path;
        public bool PathComplited { get; private set; }
        public Vector2 MoveDirection { get; private set; } = Vector2.zero;
        private int wayPointIndex = 1;


        public void SetPath(Path path)
        {
            wayPointIndex = 1;
            PathComplited = false;
            this.path = path;
            MoveDirection = GetCurrentDirection();
        }

        public Path GetPath()
        {
            return path;
        }

        public Vector2 GetNextPositon(Vector2 currentPostion, float speed)
        {
            if (path == null || wayPointIndex >= path.Count) return currentPostion;

            Vector2 newPosition = currentPostion;

            newPosition.x = Mathf.MoveTowards(currentPostion.x, path[wayPointIndex].x, speed);
            newPosition.y = Mathf.MoveTowards(currentPostion.y, path[wayPointIndex].y, speed);

            if (Vector2.Distance(newPosition, path[wayPointIndex]) < 0.0001)
            {
                wayPointIndex++;
                if (wayPointIndex >= path.Count) PathComplited = true;
                MoveDirection = GetCurrentDirection();
            }

            return newPosition;
        }

        private Vector2 GetCurrentDirection()
        {
            if (path == null || wayPointIndex >= path.Count || path.Count < 2) return Vector2.zero;

            return (path[wayPointIndex] - path[wayPointIndex]).normalized;
        }

        public bool LocalizeAgentOnPath(Vector2 currentPosition)
        {
            for (int i = 1; i < path.Count; i++)
            {
                if (path.IsBetween(i, currentPosition))
                {
                    wayPointIndex = i;
                    return true;
                }
            }

            return false;
        }
    }
}