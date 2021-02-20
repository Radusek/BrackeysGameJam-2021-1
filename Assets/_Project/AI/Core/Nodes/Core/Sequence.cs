using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Custom/AI/Sequence")]
    public class Sequence : Node
    {
        [SerializeField] private  Node[] nodes;


        public override NodeState Evaluate(EnemyAI instance)
        {
            bool isAnyRunning = false;
            foreach (var node in nodes)
            {
                switch (node.Evaluate(instance))
                {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Running:
                        isAnyRunning = true;
                        break;
                    case NodeState.Success:
                        break;
                }
            }

            return isAnyRunning ? NodeState.Running : NodeState.Success;
        }
    }
}
