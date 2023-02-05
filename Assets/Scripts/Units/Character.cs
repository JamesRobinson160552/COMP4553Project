using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//script to be attached to EVERY UNIT

public class Character : MonoBehaviour
{
    CharacterAnimator animator;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public void Moving(Vector2 moveVector)
    {
        //sets animator values so it knows what animation to
        animator.moveX = Mathf.Clamp(moveVector.x, -1f, 1f);
        animator.moveY = Mathf.Clamp(moveVector.y, -1f, 1f);

        animator.moveX = Mathf.Round(animator.moveX);
        animator.moveY = Mathf.Round(animator.moveY);

        animator.ChangeIsMoving(true);
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if(xdiff == 0 || ydiff == 0) //keeps npcs looking up, down, left or right
        {
            animator.moveX = Mathf.Clamp(xdiff, -1f, 1f);
            animator.moveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
    }

    //allows for reference of  animator through characters
    public CharacterAnimator Animator
    {
        get => animator;
    }
}
