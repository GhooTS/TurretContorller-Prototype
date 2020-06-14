using UnityEngine;

[System.Serializable]
public class WaitReaction : Reaction
{
    public float timeToWait;
    private float reactionEndTime;

    public WaitReaction(float timeToWait)
    {
        this.timeToWait = timeToWait;
    }

    public override void Start()
    {
        Debug.Log($"Start {timeToWait} reaction secound");
        reactionEndTime = Time.time + timeToWait;
    }

    public override bool Finshed()
    {
        return reactionEndTime < Time.time;
    }
}