using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Custom/AI Concrete/Move")]
    public class Move : Node
    {
        public override NodeState Evaluate(EnemyAI instance)
        {
            var movement = instance.TakeComponent<CharacterMovement>();
            var movementInfo = instance.TakeComponent<MovementInfo>();

            movement.WalkInput = movementInfo.IsGoingRight ? 1f : -1f;
            return NodeState.Success;
        }
    }
}
