using UnityEngine;
using Cinemachine;
using System.Collections.Generic;


public class CameraController : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public float transitionTime = 1f;
    private readonly List<Transform> targets = new List<Transform>(2);
    private Transform FirstTarget { get { return targets[0]; } }
    private Transform SecoundTarget { get { return targets[1]; } }

    private void Awake()
    {
        targets.Add(new GameObject("Source").transform);
        targets.Add(new GameObject("Target").transform);
        FirstTarget.hideFlags = HideFlags.HideInHierarchy;
        SecoundTarget.hideFlags = HideFlags.HideInHierarchy;
    }

    private void AddTarget(Vector2 target)
    {
        FirstTarget.position = target;
        targetGroup.AddMember(FirstTarget, 1,0);
    }

    private void AddTarget(Vector2 firstTarget, Vector2 secoundTarget)
    {
        FirstTarget.position = firstTarget;
        targetGroup.AddMember(FirstTarget, 1, 0);
        SecoundTarget.position = secoundTarget;
        targetGroup.AddMember(SecoundTarget, 1, 0);
    }

    private void AddTarget(Transform target)
    {
        targets.Add(target);
        targetGroup.AddMember(target, 1, 0);
    }

    private void Clear()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            targetGroup.RemoveMember(targets[i]);
            if (i > 1)
            {
                targets.RemoveAt(i);
            }
        }
    }

    public void StartTransition(Vector2 sourceLocation,Vector2 targetLocation)
    {
        Clear();
        AddTarget(sourceLocation,targetLocation);
    }


    public void StartTransition(Transform source, Vector2 targetLocation)
    {
        Clear();
        AddTarget(source);
        AddTarget(targetLocation);
    }

    public void StartTransition(Transform source, Transform target)
    {
        Clear();
        AddTarget(source);
        AddTarget(target);
    }

    public void StartTransition(Vector2 sourceLocation)
    {
        Clear();
        AddTarget(sourceLocation);
    }

    public void StartTransition(Transform source)
    {
        Clear();
        AddTarget(source);
    }
}
