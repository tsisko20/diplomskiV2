using RTS.Objects.Units;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RecruitPanelUI : MonoBehaviour
{
    public static RecruitPanelUI instance { get; private set; }
    private Recruiter recruiter;
    public Transform queueUI;
    public Transform currentRecruit;
    [SerializeField] private Image recruitProgressBar;
    public float totalRecruitTime;

    public void Awake()
    {
        instance = this;
    }

    public void Setup(Recruiter _recruiter)
    {
        if (recruiter != null)
        {
            recruiter.OnUnitAddedToQueue -= AddToQueueUI;
            recruiter.OnUnitRemovedFromQueue -= RemoveUnitFromQueueUI;
            recruiter.OnUnitAddedToCurrentRecruit -= UpdateCurrentRecruitUI;
            recruiter.OnUnitRemovedFromCurrentRecruit -= DeleteCurrentRecruitUI;
        }

        recruiter = _recruiter;

        recruiter.OnUnitAddedToQueue += AddToQueueUI;
        recruiter.OnUnitRemovedFromQueue += RemoveUnitFromQueueUI;
        recruiter.OnUnitAddedToCurrentRecruit += UpdateCurrentRecruitUI;
        recruiter.OnUnitRemovedFromCurrentRecruit += DeleteCurrentRecruitUI;

        UpdateQueueUI();
        UpdateCurrentRecruitUI(recruiter.currentRecruit);
    }

    public void DeleteChildren()
    {
        DeleteQueueUI();
        DeleteCurrentRecruitUI();
    }

    private void Update()
    {
        if (recruitProgressBar.IsActive() && recruiter != null)
        {
            float remainingTime = recruiter.recruitTimer;
            recruitProgressBar.fillAmount = 1 - (remainingTime / totalRecruitTime);
        }
    }

    public void AddToQueueUI(GameObject recruit)
    {
        GameObject buttonPrefab = Resources.Load<GameObject>($"Prefabs/2D/AbilityButtons/Recruit{recruit.GetComponent<Unit>().unitStats.unitName}");
        GameObject queueButton = Instantiate(buttonPrefab, queueUI);

        if (queueButton.TryGetComponent<Button>(out var btn))
        {


            btn.onClick.AddListener(() =>
            {
                recruiter.RemoveUnitFromQueue(queueButton.transform.GetSiblingIndex());
                ReturnCostToStorage(recruit);
            });
        }
    }



    public void RemoveUnitFromQueueUI(int index)
    {
        if (queueUI.childCount > 0)
            Destroy(queueUI.GetChild(index).gameObject);

    }


    public void UpdateCurrentRecruitUI(GameObject _currentRecruit)
    {

        DeleteCurrentRecruitUI();

        if (recruiter.currentRecruit == null)
        {
            recruitProgressBar.transform.parent.gameObject.SetActive(false);
            return;
        }
        recruitProgressBar.transform.parent.gameObject.SetActive(true);
        Unit recruitUnit = _currentRecruit.GetComponent<Unit>();
        totalRecruitTime = recruitUnit.unitStats.recruitTime;
        GameObject buttonPrefab = Resources.Load<GameObject>($"Prefabs/2D/AbilityButtons/Recruit{recruitUnit.unitStats.unitName}");
        GameObject currentButton = Instantiate(buttonPrefab, currentRecruit);
        if (currentButton.TryGetComponent<Button>(out var btn))
        {
            btn.onClick.AddListener(() =>
            {
                DeleteCurrentRecruitUI();
                recruiter.currentRecruit = null;
                ReturnCostToStorage(_currentRecruit);
            });
        }
    }
    public void UpdateQueueUI()
    {
        DeleteQueueUI();
        foreach (var recruit in recruiter.unitQueue)
        {
            AddToQueueUI(recruit);
        }
    }

    private void DeleteQueueUI()
    {
        foreach (Transform child in queueUI)
        {
            Destroy(child.gameObject);
        }
    }

    private void DeleteCurrentRecruitUI()
    {
        if (currentRecruit.childCount > 0)
        {
            Destroy(currentRecruit.GetChild(0).gameObject);
            if (queueUI.childCount == 0)
            {
                recruitProgressBar.transform.parent.gameObject.SetActive(false);
                Debug.Log("sakri slider");
            }
        }
    }

    private void ReturnCostToStorage(GameObject recruit)
    {
        TeamResourceStorages teamStorage = ResourceHandler.GetTeamStorage("Player");
        Unit recruitUnit = recruit.GetComponent<Unit>();
        float goldCost = recruitUnit.unitStats.baseStats.goldCost;
        float woodCost = recruitUnit.unitStats.baseStats.woodCost;
        teamStorage.UpdateResCount(ResourceType.Wood, (int)woodCost);
        teamStorage.UpdateResCount(ResourceType.Gold, (int)goldCost);
    }
}
