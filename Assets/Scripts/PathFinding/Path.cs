using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<Vector2> wayPoints;
    public float PathLenght { get; private set; }
    public Vector2 this[int i]
    {
        get { return wayPoints[i]; }
        set { wayPoints[i] = value; }
    }
    public int Count
    {
        get { return wayPoints.Count; }
    }

    public Path(List<Vector2> wayPoints)
    {
        this.wayPoints = wayPoints;
        PathLenght = 0;
        for (int i = 1; i < wayPoints.Count; i++)
        {
            PathLenght += Mathf.Abs(wayPoints[i].x - wayPoints[i - 1].x);
        }
    }

    public bool IsBetween(int index,Vector2 point)
    {
        return wayPoints[index].x <= point.x && wayPoints[index].y <= point.y
            && wayPoints[index - 1].x >= point.x && wayPoints[index - 1].y >= point.y;
    }
}
