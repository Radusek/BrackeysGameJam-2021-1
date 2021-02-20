using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Custom/AI Concrete/Turn Around")]
    public class TurnAround : Node
    {
        public override NodeState Evaluate(EnemyAI instance)
        {
            var movementInfo = instance.TakeComponent<MovementInfo>();
            var platformChecker = instance.TakeComponent<PlatformChecker>();

            bool shouldTurn;
            if (movementInfo.IsGoingRight)
                shouldTurn = platformChecker.IsAtRightWall() || platformChecker.IsOnRightEdge(); 
            else
                shouldTurn = platformChecker.IsAtLeftWall() || platformChecker.IsOnLeftEdge();

            movementInfo.IsGoingRight ^= shouldTurn;
            return shouldTurn ? NodeState.Success : NodeState.Failure;
        }
    }
}
