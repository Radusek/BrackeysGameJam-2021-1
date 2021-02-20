using UnityEngine;

namespace AI
{
    public abstract class Node : ScriptableObject
    {
        public abstract NodeState Evaluate(EnemyAI instance);
    }

    public enum NodeState
    {
        Failure,
        Running,
        Success
    }
}
