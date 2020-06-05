/// <summary>
/// Describes the common characteristics of agent
/// </summary>
[System.Serializable]
public struct AgentModel
{
    public int size;//in grid map cells
    public int height;// in grid map cells
    //Jumping
    public bool canJump;
    public int maxJumpHeight;
    //Flying
    public bool canFly;

    public AgentModel(int height, bool canJump, int maxJumpHeight,int size = 1,bool canFly = false)
    {
        this.height = height;
        this.canJump = canJump;
        this.maxJumpHeight = maxJumpHeight;
        this.size = size;
        this.canFly = canFly;
    }
}
