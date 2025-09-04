using System.Collections.Generic;
using UnityEngine;

public class TeamResourceStorages : MonoBehaviour
{
    [SerializeField] private int goldCount = 50;
    [SerializeField] private int woodCount = 50;
    [SerializeField] private ResourceUI resourceUI;
    public GameObject resStorageParent;
    public List<GameObject> allResourceStorages;

    private void Awake()
    {
        for (int i = 0; i < resStorageParent.transform.childCount; i++)
        {
            allResourceStorages.Add(resStorageParent.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        allResourceStorages.RemoveAll(item => item == null);
    }

    public void UpdateResCount(ResourceType resourceType, int value)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                goldCount += value;
                if (gameObject.tag == "Player")
                    resourceUI.UpdateResValueUI(resourceType, goldCount); break;
            case ResourceType.Wood:
                woodCount += value;
                if (gameObject.tag == "Player")
                    resourceUI.UpdateResValueUI(resourceType, woodCount); break;
        }
    }

    public void AddResStorage(GameObject resStorage)
    {
        allResourceStorages.Add(resStorage);
    }

    public int GetGoldCount()
    {
        return goldCount;
    }

    public int GetWoodCount()
    {
        return woodCount;
    }
}
