using UnityEngine;

public class IsPlayerInRange : Node
{
    private readonly Transform transform;
    private readonly Transform playerTransform;
    private readonly float range;


    public IsPlayerInRange(Transform transform, float range)
    {
        this.transform = transform;
        this.playerTransform = PlayerCharacter.Instance.transform;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        return Vector3.Distance(transform.position, playerTransform.position) > range ? NodeState.Failure : NodeState.Success;
    }
}
