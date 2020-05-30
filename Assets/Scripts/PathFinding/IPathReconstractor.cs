using System.Collections.Generic;
using UnityEngine;

public interface IPathReconstractor
{
    List<Vector2> RecreatePath(Dictionary<Node, Node> cameFrom, Node start, Node goal);
}
