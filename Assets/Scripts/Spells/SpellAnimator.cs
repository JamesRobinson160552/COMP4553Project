using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> activeFrames;
    [SerializeField] List<Sprite> setUpFrames;
    [SerializeField] bool usesSetUp;

    public bool playSetUp = false;

    SpriteAnimator activeAnim_;
    SpriteAnimator setupAnim_;
    SpriteAnimator mainAnim_;

    SpriteRenderer renderer_;

    private void Start()
    {
        renderer_ = gameObject.GetComponent<SpriteRenderer>();

        mainAnim_ = new SpriteAnimator(activeFrames, renderer_);
        if(usesSetUp)
        {
            playSetUp = true;
            setupAnim_ = new SpriteAnimator(setUpFrames, renderer_);
            activeAnim_ = setupAnim_;
        }
        else
            activeAnim_ = mainAnim_;

        activeAnim_.Start();
    }

    private void Update()
    {
        activeAnim_.HandleUpdate();
        
        if(!playSetUp)
            activeAnim_ = mainAnim_;
    }
    
}
