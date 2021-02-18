using UnityEngine;

public class RemovePauseLock : MonoBehaviour
{
    void Start() => GamePause.Instance.PauseLocks--;

    private void OnDestroy()
    {
        if (GamePause.Instance)
            GamePause.Instance.PauseLocks++;
    }
}
