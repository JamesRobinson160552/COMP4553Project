using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    float moveSpeed; 
    Rigidbody2D rb;
    Transform target_;
    Vector2 moveDirection_;
    Vector3 direction_; //Towards player
    Unit unit_;
    Boss character_;
    BossAnimator animator_;
    public float maxTimer;
    float timer; 
    int counter=0;
    bool walking;

    private void Awake()
    {
        unit_ = GetComponent<Unit>();
        character_ = GetComponent<Boss>();
        animator_ = GetComponent<BossAnimator>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target_ = GameObject.Find("Player").transform;
        moveSpeed = unit_.GetUnitBase.MoveSpeed;
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
        timer -= Time.fixedDeltaTime;

        if(!GameManager.i.gameActive || !GameManager.i.leftStartingZone)
        {
            rb.velocity = new Vector2(0, 0) * 0;
            character_.Animator.ChangeIsMoving(false);
        }
        else if (target_ && GameManager.i.insideBossRoom)
        {
            if(counter <= 50 && counter > 0) 
            {
                //CameraShake.i.StopShake();
                rb.velocity = new Vector2(moveDirection_.x, moveDirection_.y) * moveSpeed;
                character_.Moving(moveDirection_);
                if(counter >= 50)
                    counter = -25;
            }


            if(counter < 0)
            {
                rb.velocity = new Vector2(0, 0) * 0;
                character_.Animator.ChangeIsMoving(false);
            }
            counter++;
        }
    }
}
