using UnityEngine;

public class MoveToPlayer : Node
{
    private readonly Transform transform;
    private readonly Transform playerTransform;
    private readonly CharacterMovement movement;
    private readonly PlatformChecker checker;
    private readonly Rigidbody2D rb;
    private readonly float stoppingDistance;


    public MoveToPlayer(CharacterMovement movement, float minDistance)
    {
        this.transform = movement.transform;
        this.playerTransform = PlayerCharacter.Instance.transform;
        this.movement = movement;
        this.rb = movement.GetComponent<Rigidbody2D>();
        this.checker = movement.GetComponent<PlatformChecker>();
        this.stoppingDistance = minDistance;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < stoppingDistance)
            return NodeState.Success;

        var relativePos = playerTransform.position - transform.position;
        movement.WalkInput = (Mathf.Sign(relativePos.x));

        bool playerIsWayHigher = relativePos.y > 0.9f;
        bool isCloseEnough = Mathf.Abs(relativePos.x) < 2f; 
        bool shouldJump = relativePos.x > 0f ? checker.IsAtRightWall() || checker.IsOnRightEdge() : checker.IsAtLeftWall() || checker.IsOnLeftEdge();
        if (playerIsWayHigher && isCloseEnough || shouldJump)
            movement.Jump();

        return NodeState.Running;
    }
}
