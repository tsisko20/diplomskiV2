using UnityEngine;

public class MainMenuSound : MonoBehaviour
{
    public static MainMenuSound instance;
    public AudioClip musicSound;
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        PlayMusic();
    }
    public static void PlayMusic()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(instance.musicSound);
    }

}
