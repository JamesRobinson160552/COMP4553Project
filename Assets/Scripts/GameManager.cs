using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool gameActive = false;
    public GameObject titleScreen;
    public GameObject[] enemyPrefabs;
    public float spawnStartDelay = 2.0f; //Seconds
    public float spawnInterval = 12.0f; //Seconds


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameActive = true;
        titleScreen.gameObject.SetActive(false);
    }

    void SpawnEnemy()
    {
        float spawnPositionx = Random.Range(0.0f, 5.0f);
        float spawnPositiony = Random.Range(0.0f, 5.0f);
        Vector3 spawnPosition = new Vector3(spawnPositionx, spawnPositiony, 0.0f);
        Instantiate(enemyPrefabs[0], spawnPosition, enemyPrefabs[0].transform.rotation);
    }
}
