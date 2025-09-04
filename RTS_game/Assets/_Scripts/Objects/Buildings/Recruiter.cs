using RTS.Objects.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Recruiter : MonoBehaviour
{
    public List<GameObject> unitQueue = new List<GameObject>();
    public GameObject currentRecruit;
    public Unit currentUnit;
    public float recruitTimer;
    public const int maxQueueSize = 8;
    public Transform recruitPosition;
    public Transform destinationPosition;
    EnemyContext enemyContext;

    public event Action<GameObject> OnUnitAddedToQueue;
    public event Action<int> OnUnitRemovedFromQueue;
    public event Action<GameObject> OnUnitAddedToCurrentRecruit;
    public event Action OnUnitRemovedFromCurrentRecruit;

    private void Awake()
    {
        enemyContext = GameObject.Find("GameController").GetComponent<EnemyContext>();
    }
    public void AddUnitToQueue(GameObject unit)
    {
        if (unitQueue.Count < maxQueueSize)
        {
            unitQueue.Add(unit);
            OnUnitAddedToQueue?.Invoke(unit);
        }
    }

    public void RemoveUnitFromQueue(int unitNumber)
    {
        if (unitNumber < 0 || unitNumber >= unitQueue.Count)
        {
            return;
        }
        unitQueue.RemoveAt(unitNumber);
        OnUnitRemovedFromQueue?.Invoke(unitNumber);
    }

    public List<GameObject> GetQueue()
    {
        return new List<GameObject>(unitQueue);
    }

    private void Update()
    {
        if (unitQueue.Count > 0 && currentRecruit == null)
        {
            if (currentRecruit == null)
            {
                currentRecruit = unitQueue[0];
                OnUnitAddedToCurrentRecruit?.Invoke(currentRecruit);
                recruitTimer = currentRecruit.GetComponent<Unit>().unitStats.recruitTime;
                RemoveUnitFromQueue(0);
            }
        }

        if (currentRecruit == null)
            return;

        if (recruitTimer > 0)
        {
            recruitTimer -= Time.deltaTime;
        }
        else
        {
            currentUnit = currentRecruit.GetComponent<Unit>();
            recruitTimer = currentUnit.unitStats.recruitTime;
            int workersCount = enemyContext.workersParent.childCount;
            GameObject newUnit = Instantiate(currentRecruit, recruitPosition.position, recruitPosition.rotation);
            Unit newUnitComponent = newUnit.GetComponent<Unit>();
            newUnitComponent.SetTeam(gameObject.tag);
            if (newUnitComponent.tag == "Enemy" && enemyContext.workersParent.childCount > workersCount)
            {
                Vector3 workerStartPosition = newUnitComponent.transform.position;
                if (enemyContext.workersParent.childCount % 2 == 0)
                {
                    newUnitComponent.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Wood, ref workerStartPosition));
                }
                else
                {
                    newUnitComponent.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Gold, ref workerStartPosition));
                }
            }
            else
            {
                newUnitComponent.destination = destinationPosition.position;
                newUnitComponent.stateMachine.ChangeState(newUnitComponent.stateMachine.walkState);
            }
            currentRecruit = null;
            OnUnitRemovedFromCurrentRecruit?.Invoke();

        }
    }
}