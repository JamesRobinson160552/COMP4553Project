using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkLeft;
    [SerializeField] List<Sprite> walkRight;

    [SerializeField] List<Sprite> headLeft1;
    [SerializeField] List<Sprite> headLeft2;

    [SerializeField] List<Sprite> headRight1;
    [SerializeField] List<Sprite> headRight2;

    [SerializeField] List<Sprite> attackLeftLeftArm;
    [SerializeField] List<Sprite> standingLeftLeftArm;
    [SerializeField] List<Sprite> standingLeftRightArm;

    [SerializeField] List<Sprite> attackRightRightArm;
    [SerializeField] List<Sprite> standingRightRightArm;
    [SerializeField] List<Sprite> standingRightLeftArm;

    public float moveX { get; set; }
    public float moveY { get; set; }
    bool isMoving { get; set; }
    bool isAttacking { get; set; }
    bool isLoadingSpell{ get; set; }

    bool wasPreviouslyMoving_;
    bool wasPreviouslyAttacking_;
    bool isAttackingLeft_;

    SpriteAnimator walkLeftAnim_;
    SpriteAnimator walkRightAnim_;

    SpriteAnimator headLeftAnim1_;
    SpriteAnimator headLeftAnim2_;

    SpriteAnimator headRightAnim1_;
    SpriteAnimator headRightAnim2_;

    SpriteAnimator attackLeftLeftArmAnim_;
    SpriteAnimator standingLeftLeftArmAnim_;
    SpriteAnimator standingLeftRightArmAnim_;

    SpriteAnimator attackRightRightArmAnim_;
    SpriteAnimator standingRightRightArmAnim_;
    SpriteAnimator standingRightLeftArmAnim_;

    SpriteAnimator currentAnimBody_; 
    SpriteAnimator currentAnimLeftArm_;
    SpriteAnimator currentAnimRightArm_;
    SpriteAnimator currentAnimHead_;

    List<SpriteRenderer> SpriteRenderers_ = new List<SpriteRenderer>();

    private void Start()
    {
        GetComponentsInChildren(SpriteRenderers_);

        walkLeftAnim_ = new SpriteAnimator(walkLeft, SpriteRenderers_[0], 0.32f);
        walkRightAnim_ = new SpriteAnimator(walkRight, SpriteRenderers_[0], 0.32f);

        headLeftAnim1_ = new SpriteAnimator(headLeft1, SpriteRenderers_[1], 0.32f);
        headLeftAnim2_ = new SpriteAnimator(headLeft2, SpriteRenderers_[1], 0.32f);

        headRightAnim1_ = new SpriteAnimator(headRight1, SpriteRenderers_[1], 0.32f);
        headRightAnim2_ = new SpriteAnimator(headRight2, SpriteRenderers_[1], 0.32f);

        attackLeftLeftArmAnim_ = new SpriteAnimator(attackLeftLeftArm, SpriteRenderers_[2], 0.32f);
        standingLeftLeftArmAnim_ = new SpriteAnimator(standingLeftLeftArm, SpriteRenderers_[2], 0.32f);
        standingRightLeftArmAnim_ = new SpriteAnimator(standingRightLeftArm, SpriteRenderers_[2], 0.32f);

        attackRightRightArmAnim_ = new SpriteAnimator(attackRightRightArm, SpriteRenderers_[3], 0.32f);
        standingRightRightArmAnim_ = new SpriteAnimator(standingRightRightArm, SpriteRenderers_[3], 0.32f);
        standingLeftRightArmAnim_ = new SpriteAnimator(standingLeftRightArm, SpriteRenderers_[3], 0.32f);

        currentAnimBody_ = walkLeftAnim_;
        currentAnimLeftArm_ = standingLeftLeftArmAnim_;
        currentAnimRightArm_ = standingLeftRightArmAnim_;
        currentAnimHead_ =  headLeftAnim1_;
    }

    private void Update()
    {
        var prevAnim_ = currentAnimBody_;

        if(isAttacking == false)
        {
            if (moveX == 1 || (moveY == 1 && moveX == 0))
            {
                currentAnimBody_ = walkRightAnim_;
                currentAnimLeftArm_ = standingRightLeftArmAnim_;
                currentAnimRightArm_ = standingRightRightArmAnim_;
                currentAnimHead_ =  headRightAnim1_;
                isAttackingLeft_ = false;
            }

            else
            {
                currentAnimBody_ = walkLeftAnim_;
                currentAnimLeftArm_ = standingLeftLeftArmAnim_;
                currentAnimRightArm_ = standingLeftRightArmAnim_;
                currentAnimHead_ =  headLeftAnim1_;
                isAttackingLeft_ = true;
            }
        }

        else
        {
            if(!wasPreviouslyAttacking_)
            {
                Debug.Log("running");
                if(isAttackingLeft_)
                {
                    currentAnimLeftArm_ = attackLeftLeftArmAnim_;
                    SpriteRenderers_[2].sprite = currentAnimLeftArm_.Frames[1];
                }

                else
                {
                    currentAnimRightArm_ = attackRightRightArmAnim_;
                    SpriteRenderers_[3].sprite = currentAnimRightArm_.Frames[1];
                }
            }
        }

        //plays loop, first 2 lines keep animations synced
        if (isMoving || isAttacking)
        {
            if(!isAttacking)
            {
                currentAnimLeftArm_.currentFrame = currentAnimBody_.currentFrame;
                currentAnimLeftArm_.timer = currentAnimBody_.timer;

                currentAnimRightArm_.currentFrame = currentAnimBody_.currentFrame;
                currentAnimRightArm_.timer = currentAnimBody_.timer;

                currentAnimHead_.currentFrame = currentAnimBody_.currentFrame;
                currentAnimHead_.timer = currentAnimBody_.timer;
            }

            if(isAttacking && isAttackingLeft_)
            {
                currentAnimLeftArm_.HandleUpdate();
            }

            if(isAttacking && !isAttackingLeft_)
            {
                currentAnimRightArm_.HandleUpdate();
            }

            if(isMoving && isAttackingLeft_)
            {
                currentAnimBody_.HandleUpdate();
                currentAnimHead_.HandleUpdate();
                currentAnimRightArm_.HandleUpdate();
            }

            if(!isMoving && isAttackingLeft_)
            {
                currentAnimBody_.Start();
                currentAnimHead_.Start();
                currentAnimRightArm_.Start();
            }

             if(isMoving && !isAttackingLeft_)
            {
                currentAnimBody_.HandleUpdate();
                currentAnimHead_.HandleUpdate();
                currentAnimLeftArm_.HandleUpdate();
            }

            if(!isMoving && !isAttackingLeft_)
            {
                currentAnimBody_.Start();
                currentAnimHead_.Start();
                currentAnimLeftArm_.Start();
            }


        }

        else //resets animation loop when current loop is stopped
        {
            SpriteRenderers_[0].sprite = currentAnimBody_.Frames[0];
            SpriteRenderers_[1].sprite = currentAnimHead_.Frames[0];
            SpriteRenderers_[2].sprite = currentAnimLeftArm_.Frames[0];
            SpriteRenderers_[3].sprite = currentAnimRightArm_.Frames[0];
        }

        wasPreviouslyMoving_ = isMoving;
        wasPreviouslyAttacking_ = isAttacking;
    }

    public void ChangeIsMoving(bool state)
    {
        isMoving = state;
    }

    public void ChangeIsAttacking(bool state)
    {
        isAttacking = state;
    }
}
