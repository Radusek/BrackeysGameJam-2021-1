public class Selector : Node
{
    private readonly Node[] nodes;


    public Selector(params Node[] nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            var state = node.Evaluate();
            if (state != NodeState.Failure)
                return state;
        }

        return NodeState.Failure;
    }
}
