public abstract class Node
{
    public abstract NodeState Evaluate();
}
    
public enum NodeState
{
    Failure,
    Running,
    Success
}
