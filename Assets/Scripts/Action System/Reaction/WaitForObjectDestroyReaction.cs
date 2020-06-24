using UnityEngine;

public class WaitForObjectDestroyReaction : Reaction
{
    private GameObject gameObject;

    public WaitForObjectDestroyReaction(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public override bool Finshed()
    {
        return gameObject == null;
    }

    public override void Start()
    {
        
    }
}