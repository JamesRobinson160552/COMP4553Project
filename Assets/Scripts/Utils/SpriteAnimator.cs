using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script to be attached to EVERY UNIT

public class SpriteAnimator //class to render ANYTHING ANYYYYYY
{
    SpriteRenderer spriteRenderer;
    List<Sprite> frames;

    float frameRate;
    int currentFrame;
    float timer;

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate=0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameRate = frameRate;
    }

    public void Start() 
    {
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count; //resets animation loop when complete\


            //frame 0 is idle, so we want to skip that
            if(currentFrame == 0)
            {
                currentFrame++;
            }

            spriteRenderer.sprite = frames[currentFrame]; //displays current frame
            timer -= frameRate;
        }
    }

    public List<Sprite> Frames
    {
        get { return frames; }
    }
}
