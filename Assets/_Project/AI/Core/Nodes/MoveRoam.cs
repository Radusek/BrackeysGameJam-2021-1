using UnityEngine;

public class MoveRoam : Node
{
    private readonly PlatformChecker checker;
    private readonly CharacterMovement movement;
    private readonly float turningMovementDelay;
    private float sleepUntilTime;
    private bool isGoingRight;


    public MoveRoam(PlatformChecker checker, CharacterMovement movement, float turningMovementDelay)
    {
        this.checker = checker;
        this.movement = movement;
        this.turningMovementDelay = turningMovementDelay;
    }

    public MoveRoam(PlatformChecker checker, CharacterMovement movement, float turningMovementDelay, bool isGoingRight) : this(checker, movement, turningMovementDelay)
    {
        this.isGoingRight = isGoingRight;
    }

    public override NodeState Evaluate()
    {
        if (Time.time < sleepUntilTime)
            return NodeState.Running;

        bool shouldTurn;
        if (isGoingRight)
            shouldTurn = checker.IsAtRightWall() || checker.IsOnRightEdge();
        else
            shouldTurn = checker.IsAtLeftWall() || checker.IsOnLeftEdge();

        if (shouldTurn)
        {
            sleepUntilTime = Time.time + turningMovementDelay;
            isGoingRight ^= true;
            return NodeState.Running;
        }

        movement.WalkInput = isGoingRight ? 1f : -1f;
        return NodeState.Running;
    }
}
