public class Inverter : Node
{
    private readonly Node node;


    public Inverter(Node node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.Failure:
                return NodeState.Success;
            case NodeState.Running:
                return NodeState.Running;
            case NodeState.Success:
            default:
                return NodeState.Failure;
        }
    }
}
