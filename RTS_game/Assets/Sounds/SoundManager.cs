using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] unitSelectSounds;
    private void Awake()
    {
        instance = this;
    }
    public static void PlaySound(AudioClip clip)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }

    public static void PlayUnitSelectSound()
    {
        int randomSound = Random.Range(0, instance.unitSelectSounds.Length);
        PlaySound(instance.unitSelectSounds[randomSound]);
    }
}
