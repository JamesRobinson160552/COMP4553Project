using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool gameActive = false;
    public GameObject titleScreen;
    public GameObject UI;
    public GameObject menu;
    public GameObject endScreen;
    public GameObject[] enemyPrefabs;
    public bool showingDialog = false;
    public bool lightningSpawned = false;
    public SettingMenuText menuText;
    public AudioSource mainAudio;
    public AudioSource bossAudio;
    public AudioSource menuAudio;
    public AudioSource endMusic;
    public AudioClip openMenuSound;
    public AudioClip closeMenuSound;
    public AudioClip confirmSound;
    public AudioClip startGameSound;
    public List<AudioSource> audios;
    public Slider volumeSlider;
    public float volume = 1.0f;
    public float oldVolume;
    public GameObject LightningSpell;
    public GameObject BlastSpell;
    public GameObject button;
    public GameObject winScreen;
    public bool reachedBoss = false;
    

    

    //these tell the crow npc what to say.

    public bool leftStartingZone;

    public bool playLightningDialog = false;
    public bool playReflectDialog = false;
    public bool playWallDialog = false;
    public bool playKeyDialog = false;
    public bool playBlastDialog = false;
    public bool playIntroductionDialog = false;
    public bool playCaveIntroDialog = false;
    public bool firstTimeSeeingCave = true;

    public bool playerWon;

    public bool insideBossRoom = false;

    public int enemiesKilled;

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
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Noisy");
        audios.Add(GameObject.Find("Main Camera").GetComponent<AudioSource>());
        foreach (GameObject obj in temp)
        {
            foreach (AudioSource source in (obj.GetComponents<AudioSource>()))
            {
                audios.Add(source);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(playerWon == true)
        //{
        //    gameActive = false;
        //    winScreen.SetActive(true);
        //}

        if((GameObject.Find("Player").transform.position.y > -34) && !leftStartingZone)
        {
            StartCoroutine(GameObject.Find("Crow").GetComponent<Crow>().TalkToCrow());
            leftStartingZone = true;
        }

        if((GameObject.Find("Player").transform.position.x <= -55) && firstTimeSeeingCave)
        {
            playCaveIntroDialog = true;
            firstTimeSeeingCave = false;
        }

        if(enemiesKilled == 2)
        {
            playIntroductionDialog = true; 
            button.SetActive(true);
        }

        if(playIntroductionDialog == true || playCaveIntroDialog == true)
        {
            StartCoroutine(GameObject.Find("Crow").GetComponent<Crow>().TalkToCrow(true));
        }

    }

    public void StartGame()
    {
        menuAudio.PlayOneShot(startGameSound, 1.0f);
        gameActive = true;
        titleScreen.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
    }

    public void OpenMenu()
    {
        menuAudio.PlayOneShot(openMenuSound, 1.0f);
        gameActive = !gameActive;
        CameraShake.i.StopShake();
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
        insideBossRoom = false;
        BlastSpell.SetActive(true);
        LightningSpell.SetActive(true);
        DestroyAllEnemies();
        foreach (GameObject enemyGroup in enemyPrefabs)
        {
            Instantiate(enemyGroup, enemyGroup.transform.position, enemyGroup.transform.rotation);
        }
    }
    
    public void SetVolume()
    {
        oldVolume = volume;
        if (volumeSlider.value == 0)
        {
            volume = 0.0001f; //Avoid 0 division
        }
        else 
        {
            volume = volumeSlider.value;
        }
        
        foreach (AudioSource source in audios)
        {
            //resets to original value and scales by new value
            source.volume /= oldVolume;
            source.volume *= volume; 
        }
    }

    public void StartBossMusic()
    {
        mainAudio.Stop();
        bossAudio.Play();
    }

    public void EndBossMusic()
    {
        bossAudio.Stop();
        mainAudio.Play();
    }

    public void PlayMenuButtonSound()
    {
        menuAudio.PlayOneShot(confirmSound);
    }

    public void endGame()
    {
        endScreen.SetActive(true);
        gameActive = false;
        bossAudio.Stop();
        endMusic.Play();
    }
}
