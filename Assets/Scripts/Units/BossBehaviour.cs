using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    HealthBar healthBar;
    StaminaBar staminaBar;
    float ultimateAttack = 30f;
    float timer2 = 30f;

    private void Awake()
    {
        unit_ = GetComponent<Unit>();
        character_ = GetComponent<Boss>();
        animator_ = GetComponent<BossAnimator>();
    }

    private void Start()
    {
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();//transform.Find("UI").gameObject.transform.Find("BossHud").GetComponent<HealthBar>();
        staminaBar = GameObject.Find("Canvas").GetComponentInChildren<StaminaBar>();
        staminaBar.gameObject.GetComponent<Slider>().maxValue = ultimateAttack;
        //Debug.Log(healthBar.gameObject.name);
        healthBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(false);
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

        healthBar.setHealth(GetComponent<Unit>().currentHP);
        //if(timer2 != prevFrameTimer)
        //{
        //    staminaBar.setStamina(currentValue);
        //    currentValue += 1;
        //    if(currentValue == ultimateAttack)
        //    {
        //        currentValue = 0f;
        //        timer2 = 0f;
        //    }
        //    Debug.Log(currentValue);
        //}
        if(GetComponent<Unit>().currentHP <= GetComponent<BossAttacks>().maxHP/2)
        {
            timer2 -= Time.deltaTime;
            Debug.Log(updateTimer(timer2));
            staminaBar.setStamina(30 - updateTimer(timer2));
            if(updateTimer(timer2) == 0)
            {
                timer2 = ultimateAttack;
            }
        }


        if(GameManager.i.insideBossRoom)
        {
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(true);
        }
    }

    private int updateTimer(float currentTime)  //THIS IS HOW YOU MAKE REAL TIME COUNTERS!!!!!!    
    {
        //if(currentTime)
        currentTime += 1;
        Debug.Log(currentTime);

        int returnValue = Mathf.FloorToInt(currentTime % 60); 

        return returnValue;
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
