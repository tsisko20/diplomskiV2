using UnityEngine;
using UnityEngine.UI;

public class SoundScrollController : MonoBehaviour
{
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    public Image audioIcon;
    public Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        audioIcon.sprite = soundOnImage;
        slider.value = 0.5f;
        AudioListener.volume = 0.5f;
    }
    public void ChangeVolume()
    {
        AudioListener.volume = slider.value;
        if (slider.value == 0)
        {
            audioIcon.sprite = soundOffImage;
            audioIcon.color = Color.darkRed;
        }
        else
        {
            audioIcon.sprite = soundOnImage;
            audioIcon.color = Color.black;
        }
    }



}
