using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;

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
        if (CameraControl.IsCameraControllOn())
            CameraControl.ToogleCameraControl();
    }
}
