using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
    public void OnContinueButtonClick()
    {
        pauseUI.SetActive(false);
    }
    public void OnMenuButtonClick()
    {
        pauseUI.SetActive(true);
    }
}
