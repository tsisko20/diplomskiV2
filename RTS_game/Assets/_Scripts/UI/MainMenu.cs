using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("UnitTestEnvironment");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
