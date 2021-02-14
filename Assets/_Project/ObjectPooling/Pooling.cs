using UnityEngine;
using System.Collections.Generic;

public static class Pooling
{
    public const int DEFAULT_POOL_SIZE = 3;
    public static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    public class Pool
    {
        private readonly GameObject prefab;
        private readonly Queue<GameObject> inactiveObjects;
        private int nextId = 1;

        public readonly HashSet<int> memberIDs;


        public Pool(GameObject prefab, int initialQuantity)
        {
            this.prefab = prefab;
            inactiveObjects = new Queue<GameObject>(initialQuantity);
            memberIDs = new HashSet<int>();
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
        {
            GameObject obj;
            if (inactiveObjects.Count == 0)
            {
                obj = Object.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                memberIDs.Add(obj.GetInstanceID());
            }
            else
            {
                obj = inactiveObjects.Dequeue();

                if (obj == null)
                {
                    return Spawn(pos, rot, parent);
                }
            }

            obj.transform.SetParent(parent, false);
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);

            return obj;
        }

        public void Despawn(GameObject obj)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
                obj.transform.parent = null;
                inactiveObjects.Enqueue(obj);
            }
        }
    }

    private static void Init(GameObject prefab = null, int quantity = DEFAULT_POOL_SIZE)
    {
        if (prefab != null)
        {
            var prefabID = prefab.GetInstanceID();
            if (!pools.ContainsKey(prefabID))
                pools[prefabID] = new Pool(prefab, quantity);
        }
    }

    public static void Preload(GameObject prefab, int quantity = 1)
    {
        Init(prefab, quantity);

        for (int i = 0; i < quantity; i++)
        {
            Despawn(Spawn(prefab, Vector3.zero, Quaternion.identity));
        }
    }

    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        Init(prefab);
        return pools[prefab.GetInstanceID()].Spawn(pos, rot, parent);
    }

    public static T Spawn<T>(T component, Vector3 pos, Quaternion rot, Transform parent = null) where T : Component
    {
        return Spawn(component.gameObject, pos, rot, parent).GetComponent<T>();
    }

    public static GameObject Spawn(GameObject prefab, Vector3 pos, Transform parent = null)
    {
        Init(prefab);
        return pools[prefab.GetInstanceID()].Spawn(pos, Quaternion.identity, parent);
    }

    public static T Spawn<T>(T component, Vector3 pos, Transform parent = null) where T : Component
    {
        return Spawn(component.gameObject, pos, parent).GetComponent<T>();
    }

    public static GameObject Spawn(GameObject prefab, Transform parent = null)
    {
        Init(prefab);
        return pools[prefab.GetInstanceID()].Spawn(Vector3.zero, Quaternion.identity, parent);
    }

    public static T Spawn<T>(T component, Transform parent = null) where T : Component
    {
        return Spawn(component.gameObject, parent).GetComponent<T>();
    }

    public static void Despawn(GameObject go)
    {
        Pool assignedPool = null;
        foreach (var pool in pools.Values)
        {
            if (pool.memberIDs.Contains(go.GetInstanceID()))
            {
                assignedPool = pool;
                break;
            }
        }

        if (assignedPool == null)
        {
            Object.Destroy(go);
        }
        else
        {
            assignedPool.Despawn(go);
        }
    }

    public static void Despawn(GameObject go, float delay)
    {
        MEC.Timing.RunCoroutine(DespawnDelayed(go, delay));
    }

    private static IEnumerator<float> DespawnDelayed(GameObject go, float delay)
    {
        yield return MEC.Timing.WaitForSeconds(delay);
        Despawn(go);
    }
}

public static class SimplePoolGameObjectExtensions
{
    public static void Despawn(this GameObject go)
    {
        Pooling.Despawn(go);
    }

    public static void Despawn(this GameObject go, float delay)
    {
        Pooling.Despawn(go, delay);
    }
}