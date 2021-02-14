using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string SavePath => $"{Application.persistentDataPath}/save.sav";
    private static Dictionary<string, object> data = new Dictionary<string, object>();
    private static Coroutine saveCoroutine;
    private static IFormatter formatter;

    public static SaveManager Instance { get; private set; }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Instance = new GameObject("SaveManager", typeof(SaveManager)).GetComponent<SaveManager>();
        DontDestroyOnLoad(Instance.gameObject);
        formatter = GetFormatter();
    }

    private void Awake() => data = LoadFile();

    public static T Get<T>(string key, T def = default)
    {
        if (data.ContainsKey(key))
            return (T)data[key];

        return def;
    }

    public static void Set<T>(string key, T value)
    {
        data[key] = value;
        Save();
    }

    private static void Save()
    {
        if (saveCoroutine != null)
            Instance.StopCoroutine(saveCoroutine);

        saveCoroutine = Instance.StartCoroutine(SaveCoroutine());
    }

    public static void DeleteAll()
    {
        data = null;
        SaveFile(null);
    }

    private static IEnumerator SaveCoroutine()
    {
        yield return null;
        SaveFile(data);
    }

    private static void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = GetFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private static Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (var stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = GetFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private static IFormatter GetFormatter()
    {
        var formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();
        Vector3Surrogate vector3Surrogate = new Vector3Surrogate();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
