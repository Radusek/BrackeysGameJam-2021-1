using System.Collections;
using UnityEngine;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BehaviourProvider behaviourProvider;

        private Node behaviour;


        private IEnumerator Start()
        {
            AssignNewBehaviour();
            yield return new WaitForSeconds(1f);
            while (true)
            {
                yield return null;
                if (GamePause.Instance.IsPaused)
                    continue;

                behaviour.Evaluate();
            }
        }

        [ContextMenu("Get Behaviour")]
        public void AssignNewBehaviour()
        {
            behaviour = behaviourProvider.GetBehaviour(transform);
        }
    }
}
