using UnityEngine;

[CreateAssetMenu(menuName = "Custom/AI behaviours/Dummy Enemy")]
public class DummyEnemyBehaviour : BehaviourProvider
{
    [SerializeField] private float playerChasingRange;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float turnAroundIdlingTime;


    public override Node GetBehaviour(Transform transform)
    {
        var rangeNode = new IsPlayerInRange(transform, playerChasingRange);
        var moveNode = new MoveToPlayer(transform.GetComponent<CharacterMovement>(), stoppingDistance);
        var chaseSequence = new Sequence(rangeNode, moveNode);

        var roamNode = new MoveRoam(transform.GetComponent<PlatformChecker>(), transform.GetComponent<CharacterMovement>(), turnAroundIdlingTime);

        return new Selector(chaseSequence, roamNode);
    }
}
