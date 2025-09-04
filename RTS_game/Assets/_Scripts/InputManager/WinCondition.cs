using RTS.Objects;
using TMPro;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public static WinCondition instance { get; private set; }
    [SerializeField] GameObject EndScreen;
    [SerializeField] TextMeshProUGUI resultValue;
    [SerializeField] Transform playerFarmHouses;
    [SerializeField] Transform playerBarracks;
    [SerializeField] Transform enemyFarmHouses;
    [SerializeField] Transform enemyBarracks;
    [SerializeField] bool playerAlive = false;
    [SerializeField] bool enemyAlive = false;

    private void Awake()
    {
        instance = this;
        playerFarmHouses = GameObject.Find("Player/Buildings/Resource Storages").transform;
        playerBarracks = GameObject.Find("Player/Buildings/Barracks").transform;
        enemyFarmHouses = GameObject.Find("Enemy/Buildings/Resource Storages").transform;
        enemyBarracks = GameObject.Find("Enemy/Buildings/Barracks").transform;
    }
    public static void CallTestWinCondition()
    {
        instance.TestWinCondition();
    }
    public void TestWinCondition()
    {
        playerAlive = false;
        if (playerFarmHouses.childCount != 0 || playerBarracks.childCount != 0)
        {
            playerAlive = CheckAliveBuildings(playerFarmHouses);
            if (playerAlive == false)
                playerAlive = CheckAliveBuildings(playerBarracks);
        }
        if (playerAlive == false)
            GameLost();
        enemyAlive = false;
        if (enemyFarmHouses.childCount != 0 || enemyBarracks.childCount != 0)
        {
            enemyAlive = CheckAliveBuildings(enemyFarmHouses);
            if (enemyAlive == false)
                enemyAlive = CheckAliveBuildings(enemyBarracks);
        }
        if (enemyAlive == false)
            GameWon();
    }

    private bool CheckAliveBuildings(Transform parentObject)
    {
        bool alive = false;
        foreach (Transform barracks in parentObject)
        {
            SelectableObject selectable = barracks.GetComponent<SelectableObject>();
            if (selectable.IsDead() == false)
            {
                alive = true;
            }
        }
        return alive;
    }

    private void GameLost()
    {
        resultValue.text = "Lost";
        resultValue.color = Color.darkRed;
        EndScreen.SetActive(true);
        GameObject.Find("Player").SetActive(false);
        GameObject.Find("Enemy").SetActive(false);
        GameObject.Find("GameController").SetActive(false);
        CameraControl.instance.enabled = false;
    }

    private void GameWon()
    {
        resultValue.text = "Won";
        resultValue.color = Color.darkGreen;
        EndScreen.SetActive(true);
        GameObject.Find("Player").SetActive(false);
        GameObject.Find("Enemy").SetActive(false);
        GameObject.Find("GameController").SetActive(false);
        CameraControl.instance.enabled = false;
    }
}
