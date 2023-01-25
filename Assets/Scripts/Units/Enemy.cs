using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed; 
    public float attackRange;
    public float attackSpeed = 0.75f;
    public float shootDelay = 0.5f;
    public int damage = 1;
    public float bulletForce = 5.0f;
    public GameObject attackPrefab;
    Rigidbody2D rb;
    Transform target_;
    Vector2 moveDirection_;
    Vector3 direction_; //Towards player
    Unit unit_;

    private void Awake()
    {
        unit_ = GetComponent<Unit>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target_ = GameObject.Find("Player").transform;
        moveSpeed = unit_.GetUnitBase.MoveSpeed;
        InvokeRepeating("Shoot", shootDelay, attackSpeed);
    }

    private void Update()
    {
        if(target_)
        {
            direction_ = (target_.position - transform.position).normalized;
            moveDirection_ = direction_;
        }
    }

    private void FixedUpdate()
    {
        if(target_)
        {
            rb.velocity = new Vector2(moveDirection_.x, moveDirection_.y) * moveSpeed;
        }
    }

    private void Shoot() {
        if ((direction_).magnitude <= attackRange) //Player is in range
        {
            // Instantiates bullet at location of aimer
            GameObject bullet = Instantiate(attackPrefab, transform.position, attackPrefab.transform.rotation);

            // Access the bullet's rigidbody and store it as rb
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            //give the projectile the stats from the sepll
            bullet.GetComponent<ProjectileStats>().SetDamage(damage);

            // Add force to the newly instantiated rb
            rb.AddForce(direction_ * bulletForce, ForceMode2D.Impulse);
        }
    }

}
