using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonUI : MonoBehaviour
{
    [SerializeField] private Image cooldownIcon;
    [SerializeField] private Image activeImage;
    private float cdDuration;
    private float duration;
    private bool abilityIsOnCd;
    void Start()
    {
        cooldownIcon.fillAmount = 0;
        activeImage.fillAmount = 0;
    }

    public void Setup(float _cdDuration, float _duration)
    {
        cdDuration = _cdDuration;
        duration = _duration;
    }

    public void UpdateActiveAnimation(float activeDuration)
    {
        activeImage.fillAmount = duration > 0 ? 1 - activeDuration / duration : 0;
    }
    public void UpdateCdAnimation(float activeCooldown)
    {
        cooldownIcon.fillAmount = cdDuration > 0 ? 1 - activeCooldown / cdDuration : 0;
    }
}
