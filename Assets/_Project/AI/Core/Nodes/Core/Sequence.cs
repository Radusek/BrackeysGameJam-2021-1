public class Sequence : Node
{
    private readonly Node[] nodes;


    public Sequence(params Node[] nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    break;
            }
        }

        return  NodeState.Success;
    }
}

