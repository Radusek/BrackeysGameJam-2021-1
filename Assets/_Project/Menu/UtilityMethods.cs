using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityMethods : MonoBehaviour
{
    public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);

    public void QuitApplication() => Application.Quit();
}
