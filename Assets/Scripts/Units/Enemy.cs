using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed; 
    public float attackRange;
    public float visionRange; //How close player gets to initiate tracking
    private float distanceToPlayer;
    public float attackSpeed = 0.75f;
    public float shootDelay = 0.5f;
    public float castLoop = 0.0f;
    public float castTime = 1.25f;
    public int damage = 1;
    public float bulletForce = 5.0f;
    public GameObject[] attackPrefab;
    Rigidbody2D rb;
    Transform target_;
    Vector2 moveDirection_;
    Vector3 direction_; //Towards player
    Unit unit_;
    Character character_;
    public bool castLightning;

    private void Awake()
    {
        unit_ = GetComponent<Unit>();
        character_ = GetComponent<Character>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target_ = GameObject.Find("Player").transform;
        moveSpeed = unit_.GetUnitBase.MoveSpeed;
        if(castLightning)
         InvokeRepeating("UseLightning", shootDelay, attackSpeed);
        else
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
        if(GameManager.i.gameActive == false)
        {
            rb.velocity = new Vector2(0, 0) * 0;
            character_.Animator.ChangeIsMoving(false);
        }
        else if (target_)
        {
            distanceToPlayer = Vector3.Distance(target_.position, transform.position);
            if (distanceToPlayer <= visionRange)
            {
                rb.velocity = new Vector2(moveDirection_.x, moveDirection_.y) * moveSpeed;
                character_.Moving(moveDirection_);
            }
            else
            {
                rb.velocity = new Vector2(0, 0) * 0;
                character_.Animator.ChangeIsMoving(false);
            }
        }
    }

    private void Shoot() {
        if (distanceToPlayer <= attackRange && GameManager.i.gameActive == true) //Player is in range
        {
            // Instantiates bullet at location of aimer
            GameObject bullet = Instantiate(attackPrefab[0], transform.position, attackPrefab[0].transform.rotation);

            // Access the bullet's rigidbody and store it as rb
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            //give the projectile the stats from the sepll
            bullet.GetComponent<ProjectileStats>().SetDamage(damage);

            // Add force to the newly instantiated rb
            rb.AddForce(direction_ * bulletForce, ForceMode2D.Impulse);
        }
    }

    private void UseLightning() {
        if (distanceToPlayer <= attackRange && GameManager.i.gameActive == true)
        {
            GameObject lightning = Instantiate(attackPrefab[1], target_.position, attackPrefab[0].transform.rotation);
            lightning.GetComponent<ProjectileStats>().SetDestructTimer(castTime + 0.2f);
            lightning.GetComponent<ProjectileStats>().CauseCameraShake(true, true, 0.01f);
            GetComponent<EnemyLightning>().SetData(true, damage, castLoop, castTime, lightning);
        }
    }

}
