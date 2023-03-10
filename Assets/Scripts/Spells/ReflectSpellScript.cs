using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectSpellScript : MonoBehaviour, SpellBase
{
    [SerializeField] Sprite spellIcon;
    private float lifeSpan = 3f; 
    public float lifeRemaining;
    public bool playerAccess;
    public List<char> spellActivate = new List<char> {'A', 'S', 'D', 'S' };
    public string spellName = "Wall";
    public GameObject[] spellPrefabs;
    public GameObject plr;
    public string desc;
    public AudioSource reflectAudio;

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
        reflectAudio.Play();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        // Need TAN to get angle, thereofre need Opposite / Adjecent of sides between plr and mouse
        Vector3 plrPos = Camera.main.ScreenToWorldPoint(plr.transform.position);
        float adjacent = mousePos.x - plr.transform.position.x;
        float opposite = mousePos.y - plr.transform.position.y;
        float angle = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
        Vector3 angleVector = new Vector3(0, 0, angle);
        //Debug.Log("Angle: " + angle);
        Debug.Log("Cast Angle Vector: " + angleVector);


        GameObject wall = Instantiate(spellPrefabs[0], mousePos, Quaternion.Euler(angleVector));
        wall.GetComponent<DestroyMe>().SetLife(lifeSpan);
        wall.gameObject.layer = LayerMask.NameToLayer("Reflect");
        wall.GetComponent<WallAnimator>().breakingPoint = 1f; //when cracks form
        wall.transform.localScale = new Vector3(0.5f, 2.5f, 1f);

    }
}
