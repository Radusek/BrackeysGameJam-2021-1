using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField] private MenuController menuController;

    private float prePauseTimeScale;

    public bool IsPaused { get; private set; }
    public int PauseLocks { get; set; } = 1;
    public static GamePause Instance { get; private set; }


    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        Instance = Instantiate(Resources.Load<GamePause>("GamePause"));
        DontDestroyOnLoad(Instance.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuController.gameObject.activeSelf)
                SetGamePaused(true);
            else if(menuController.IsAtRootSubmenu)
                SetGamePaused(false);
        }
    }

    public void SetGamePaused(bool state)
    {
        if (PauseLocks > 0 || state == IsPaused)
            return;

        SetPausePanelVisible(state);
        ManageTimeScale(state);
        IsPaused = state;
    }

    private void SetPausePanelVisible(bool state) => menuController.gameObject.SetActive(state);

    private void ManageTimeScale(bool state)
    {
        if (state)
            prePauseTimeScale = Time.timeScale;
        else
            Time.timeScale = prePauseTimeScale;
    }
}
