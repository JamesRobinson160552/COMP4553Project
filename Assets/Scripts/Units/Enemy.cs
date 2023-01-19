using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float moveSpeed; 
    public Rigidbody2D rb;
    Transform target_;
    Vector2 moveDirection_;
    Unit unit_;

    private void Awake()
    {
        unit_ = GetComponent<Unit>();
    }

    private void Start()
    {
        target_ = GameObject.Find("Player").transform;
        moveSpeed = unit_.GetUnitBase.MoveSpeed;
    }

    private void Update()
    {
        if(target_)
        {
            Vector3 direction_ = (target_.position - transform.position).normalized;
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

}
