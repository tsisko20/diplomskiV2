using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldValueUI;
    [SerializeField] private TextMeshProUGUI woodValueUI;

    public void UpdateResValueUI(ResourceType resType, int value)
    {
        switch (resType)
        {
            case ResourceType.Gold:
                goldValueUI.text = value.ToString(); break;
            case ResourceType.Wood:
                woodValueUI.text = value.ToString(); break;
        }
    }


}
