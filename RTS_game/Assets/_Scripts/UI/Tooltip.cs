using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI tooltipText;
    public static Tooltip instance { get; private set; }
    private RectTransform backgroundTransform;
    private RectTransform rectTransform;
    [SerializeField] private RectTransform canvasTransform;
    private void Awake()
    {
        instance = this;
        backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        canvasTransform = transform.parent.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasTransform.localScale.x;

        if (anchoredPosition.x + backgroundTransform.rect.width > canvasTransform.rect.width)
        {
            anchoredPosition.x = canvasTransform.rect.width - backgroundTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundTransform.rect.height > canvasTransform.rect.height)
        {
            anchoredPosition.y = canvasTransform.rect.height - backgroundTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string text)
    {
        tooltipText.SetText(text);
        tooltipText.ForceMeshUpdate();
        Vector2 paddingSize = new Vector2(8, 8);
        Vector2 backgroundSize = tooltipText.GetRenderedValues(false);
        backgroundTransform.sizeDelta = backgroundSize + paddingSize;
    }

    private void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        SetText(text);
    }

    public static void Show(string text)
    {
        instance.ShowTooltip(text);
    }

    public static void Hide()
    {
        instance.HideTooltip();
    }




    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
