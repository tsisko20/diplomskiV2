using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TeamResourceStorages : MonoBehaviour
{
    private int goldCount;
    private int woodCount;
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
                resourceUI.UpdateResValueUI(resourceType, goldCount); break;
            case ResourceType.Wood:
                woodCount += value;
                resourceUI.UpdateResValueUI(resourceType, woodCount); break;
        }
    }
}
