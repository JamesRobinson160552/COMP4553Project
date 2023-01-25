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

    //allows for reference of  animator through characters
    public CharacterAnimator Animator
    {
        get => animator;
    }
}
