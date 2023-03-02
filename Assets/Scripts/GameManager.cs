using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool gameActive = false;
    public GameObject titleScreen;
    public GameObject UI;
    public GameObject menu;
    public GameObject[] enemyPrefabs;
    public bool showingDialog = false;
    public bool lightningSpawned = false;
    public SettingMenuText menuText;

    public bool leftStartingZone;

    public bool playLightningDialog = false;

    public static GameManager i { get; set; }

    public SettingMenuText MenuText
    {
        get => menuText;
    }

    private void Awake()
    {
        i = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if((GameObject.Find("Player").transform.position.y > -34) && !leftStartingZone)
        {
            GameObject.Find("Crow").GetComponent<Crow>().TalkToCrow();
            leftStartingZone = true;
        }
    }

    public void StartGame()
    {
        gameActive = true;
        titleScreen.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
    }

    public void OpenMenu()
    {
        gameActive = !gameActive;
        menu.gameObject.SetActive(!gameActive);
        Debug.Log("menu open");
    }

    void DestroyAllEnemies()
    {
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in aliveEnemies) {
            GameObject.Destroy(enemy);
        }
    }

    public void ResetEnemies()
    {
        DestroyAllEnemies();
        foreach (GameObject enemyGroup in enemyPrefabs)
        {
            Instantiate(enemyGroup, enemyGroup.transform.position, enemyGroup.transform.rotation);
        }
    }
}
