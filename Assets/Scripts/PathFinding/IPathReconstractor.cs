using System.Collections.Generic;
using UnityEngine;

namespace Nav2D
{
    public interface IPathReconstractor
    {
        List<Vector2> RecreatePath(Dictionary<Vector3Int, Node> cameFrom, Node start, Node goal, NavGrid navGrid);
    }
}