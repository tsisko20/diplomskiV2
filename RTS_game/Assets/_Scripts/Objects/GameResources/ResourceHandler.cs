using NUnit.Framework;
using RTS;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ResourceHandler : MonoBehaviour
{
    public static ResourceHandler instance;

    public TeamResourceStorages playerResStorage;
    public TeamResourceStorages enemyResStorage;
    public GameObject goldObjectsParent;
    public GameObject treeObjectsParent;
    public List<GameObject> allGoldObjects;
    public List<GameObject> allTreeObjects;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < goldObjectsParent.transform.childCount; i++)
        {
            allGoldObjects.Add(goldObjectsParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < treeObjectsParent.transform.childCount; i++)
        {
            allTreeObjects.Add(treeObjectsParent.transform.GetChild(i).gameObject);
        }
    }
    public void UpdateResStorage(Team team, ResourceType resType, int value)
    {
        switch (team)
        {
            case Team.Player:
                playerResStorage.UpdateResCount(resType, value); break;
            case Team.Enemy:
                enemyResStorage.UpdateResCount(resType, value); break;
        }
    }
    private TeamResourceStorages GetTeamStorageByTag(string team)
    {
        switch (team)
        {
            case "Player":
                return playerResStorage; break;
            case "Enemy":
                return enemyResStorage; break;
            default: return null;
        }
    }
    public static TeamResourceStorages GetTeamStorage(string team)
    {
        return instance.GetTeamStorageByTag(team);
    }
}
