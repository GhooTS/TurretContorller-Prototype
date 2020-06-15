using System.Collections.Generic;
using UnityEngine;

namespace Nav2D
{
    public class Path
    {
        private List<Vector2> waypoints;
        public float Distance { get; private set; }
        public Vector2 Start
        {
            get { return waypoints[0]; }
        }
        public Vector2 End
        {
            get { return waypoints[waypoints.Count - 1]; }
        }
        public Vector2 this[int i]
        {
            get { return waypoints[i]; }
            set { waypoints[i] = value; }
        }
        public int Count
        {
            get { return waypoints.Count; }
        }

        public Path(List<Vector2> waypoints)
        {
            this.waypoints = waypoints;
            Distance = 0;

            for (int i = 1; i < waypoints.Count; i++)
            {
                Distance += Mathf.Abs(waypoints[i].x - waypoints[i - 1].x);
            }
        }


        public void Combine(Path path)
        {
            if (path == null) return;

            waypoints.AddRange(path.waypoints);
            Distance += path.Distance;
        }

        public bool IsBetween(int index, Vector2 point)
        {
            point.Normalize();
            return Vector2.Dot(waypoints[index].normalized, point) > 0.999f
                && Vector2.Dot(waypoints[index - 1].normalized, point) < -0.999f;

            //return wayPoints[index].x <= point.x && wayPoints[index].y <= point.y
            //    && wayPoints[index - 1].x >= point.x && wayPoints[index - 1].y >= point.y;
        }
    }
}