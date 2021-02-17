using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource musicAudioSource;

    public float SFXVolume { get; set; }
    public static AudioManager Instance { get; private set; }


    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        Instance = Instantiate(Resources.Load<AudioManager>("AudioManager"));
        DontDestroyOnLoad(Instance.gameObject);
    }

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();

        SaveManager.Set("SFXVolume", SaveManager.Get("SFXVolume", 0.5f));
        SaveManager.Set("MusicVolume", SaveManager.Get("MusicVolume", 0.5f));

        SFXVolume = SaveManager.Get<float>("SFXVolume");
        SetMusicVolume(SaveManager.Get<float>("MusicVolume"));
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = 0.4f * volume;
    }
}
