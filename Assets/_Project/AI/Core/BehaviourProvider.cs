using UnityEngine;

public abstract class BehaviourProvider : ScriptableObject
{
    public abstract Node GetBehaviour(Transform transform);
}
