using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Custom/AI/Selector")]
    public class Selector : Node
    {
        [SerializeField] private  Node[] nodes;


        public override NodeState Evaluate(EnemyAI instance)
        {
            foreach (var node in nodes)
            {
                var state = node.Evaluate(instance);
                if (state != NodeState.Failure)
                    return state;
            }

            return NodeState.Failure;
        }
    }
}