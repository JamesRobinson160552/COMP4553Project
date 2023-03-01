using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Autoattack : MonoBehaviour, SpellBase
{
    public Camera cam;
    public Transform aimer;
    public GameObject AABullet;
    public Rigidbody2D aimerRB;
    Vector2 movement;  // Stores x and y 
    public float moveSpeed = 5.0f;
    public GameManager gameManager;
    public GameObject plr;
    public int damage = 5;
    float timer = 0;

    public float bulletForce = 20f;

    Vector2 mousePos;

    public List<char> spellActivate = new List<char> {'Z', 'Z', 'Z', 'Z' };

    public string getName()
    { return "Auto Attack"; }

    public string getDesc()
    { return ""; }

    public bool playerHasAccess()
    { return true; }

    public void setPlayerAccess()
    {}

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return damage; }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {

       if (gameManager.gameActive)
        {
            //Only allow movement when game is active 
            //aimerRB.MovePosition(aimerRB.position + movement * moveSpeed * Time.fixedDeltaTime);
            Vector2 plrPos = plr.transform.position;
            aimerRB.MovePosition(plrPos);

            // What to instantiate, where to instantiate it, and what direction it should face

            // Rotates the transparent aimerRB to direction of mouse
            Vector2 fireDir = mousePos - aimerRB.position;
            float angle = Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg - 90f; // angle between x axis, and 2D vector between 0,0 and x,y
            aimerRB.rotation = angle;

            if(timer < 0)
            {
                //Debug.Log(timer);
                CameraShake.i.StopShake();
            }

            timer -= Time.deltaTime;
        }

    }
    public void castSpell()
    {
        // Instantiates bullet at location of aimer
        GameObject bullet = Instantiate(AABullet, aimer.position, aimer.rotation);

        // Access the bullet's rigidbody and store it as rb
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //give the projectile the stats from the sepll
        bullet.GetComponent<ProjectileStats>().SetDamage(damage);
        CameraShake.i.Shake(1f);
        timer = 0.1f;

        // Add force to the newly instantiated rb
        rb.AddForce(aimer.up * bulletForce, ForceMode2D.Impulse);
    }
}
