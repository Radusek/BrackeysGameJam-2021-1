using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Custom/AI/Inverter")]
    public class Inverter : Node
    {
        [SerializeField] private  Node node;


        public override NodeState Evaluate(EnemyAI instance)
        {
            switch (node.Evaluate(instance))
            {
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    return NodeState.Failure;
                default:
                    return NodeState.Failure;
            }
        }
    }
}
