//TODO : Come up with better name for class 
/// <summary>
/// Derive from this base class to create queueable reaction
/// </summary>
public abstract class Reaction
{
    /// <summary>
    /// Start execution of reaction
    /// </summary>
    public abstract void Start();
    /// <summary>
    /// Checked if reaction has finished
    /// </summary>
    /// <returns></returns>
    public abstract bool Finshed();
}
