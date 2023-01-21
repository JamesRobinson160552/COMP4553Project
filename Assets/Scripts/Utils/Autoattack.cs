using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Autoattack : MonoBehaviour
{
    public Camera cam;
    public Transform aimer;
    public GameObject AABullet;
    public Rigidbody2D aimerRB;
    Vector2 movement;  // Stores x and y 
    public float moveSpeed = 5.0f;
    public GameManager gameManager;
    public GameObject plr;

    public float bulletForce = 20f;

    Vector2 mousePos;

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Shoot();
        //}
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
        }

    }
    public void Shoot()
    {
        // Instantiates bullet at location of aimer
        GameObject bullet = Instantiate(AABullet, aimer.position, aimer.rotation);

        // Access the bullet's rigidbody and store it as rb
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Add force to the newly instantiated rb
        rb.AddForce(aimer.up * bulletForce, ForceMode2D.Impulse);
    }

    public void SpecialShoot()
    {
        // Instantiates bullet at location of aimer
        GameObject bullet = Instantiate(AABullet, aimer.position, aimer.rotation);
        bullet.GetComponent<SpriteRenderer>().material.color = new Color(256, 256, 256, 1);

        // Access the bullet's rigidbody and store it as rb
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Add force to the newly instantiated rb
        rb.AddForce(aimer.up * bulletForce, ForceMode2D.Impulse);
    }
}
