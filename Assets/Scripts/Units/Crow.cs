using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    Transform playerPos_;
    Character character_;
    Vector2 direction_;
    Vector3 target_;
    Vector3 crowPositon;
    Rigidbody2D rb;

    float moveSpeed;

    void Start()
    {
        playerPos_ = GameObject.Find("Player").transform;
        character_ = GetComponent<Character>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        target_ = new Vector3(playerPos_.position.x, playerPos_.position.y + 1.7f, playerPos_.position.z);
        moveSpeed = GameObject.Find("Player").GetComponent<PlayerController>().currentMoveSpeed - 0.2f;
        direction_ = (target_ - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if((transform.position.x - 0.1f < target_.x && target_.x < transform.position.x + 0.1f) && (transform.position.y - 0.1f < target_.y && target_.y < transform.position.y + 0.1f))
        {
            character_.Animator.ChangeIsMoving(false);
            rb.velocity = new Vector2(0, 0) * 0;
        }

        else
        {
            rb.velocity = new Vector2(direction_.x, direction_.y) * moveSpeed;
            character_.Moving(direction_);
        }
    }
}
