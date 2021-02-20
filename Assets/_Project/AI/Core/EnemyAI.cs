using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Node behaviour;

        private Dictionary<System.Type, Component> components;

        public float SleepingEndTime { get; set; }


        private void Awake()
        {
            components = new Dictionary<System.Type, Component>();
            foreach (var component in GetComponents<Component>())
            {
                components[component.GetType()] = component;
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                yield return null;
                behaviour.Evaluate(this);
            }
        }

        public T TakeComponent<T>() where T : Component
        {
            return components[typeof(T)] as T;
        }
    }
}
