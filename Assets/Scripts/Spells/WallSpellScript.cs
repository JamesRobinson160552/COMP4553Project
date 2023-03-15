using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

//Destroys the wall created after a given length of time;
public class WallSpellScript : MonoBehaviour, SpellBase
{
    [SerializeField] Sprite spellIcon;
    private float lifeSpan = 5.0f; 
    public float lifeRemaining;
    public bool playerAccess;
    public List<char> spellActivate = new List<char> {'A', 'A', 'A', 'A' };
    public string spellName = "Wall";
    public GameObject[] spellPrefabs;
    public GameObject plr;
    public string desc;
    public AudioSource wallAudio;

    public Sprite getIcon()
    {
        return spellIcon;
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeRemaining = lifeSpan;
    }

    public string getName()
    { return spellName; }

    public string getDesc()
    {
        return desc;
    }

    public bool playerHasAccess()
    { return playerAccess; }

    public void setPlayerAccess()
    { playerAccess = !playerAccess; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return 0; }


    public void castSpell()
    {
        wallAudio.Play();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        // Need TAN to get angle, thereofre need Opposite / Adjecent of sides between plr and mouse
        Vector3 plrPos = Camera.main.ScreenToWorldPoint(plr.transform.position);
        float adjacent = mousePos.x - plr.transform.position.x;
        float opposite = mousePos.y - plr.transform.position.y;
        float angle = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
        Vector3 angleVector = new Vector3(0, 0, angle);


        GameObject wall = Instantiate(spellPrefabs[0], mousePos, Quaternion.Euler(angleVector));
        wall.GetComponent<WallAnimator>().breakingPoint = 2f;
        wall.GetComponent<DestroyMe>().SetLife(lifeSpan);
    }
}
