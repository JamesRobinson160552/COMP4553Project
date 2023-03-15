using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    public float rotateSpeed=0;
    float newRotation;

     SpriteAnimator Anim_;
    // Start is called before the first frame update
    List<SpriteRenderer> SpriteRenderers_ = new List<SpriteRenderer>();

    private void Start()
    {
        newRotation = gameObject.transform.rotation.z;
        GetComponentsInChildren(SpriteRenderers_);

        Anim_ = new SpriteAnimator(sprites, SpriteRenderers_[0]);

        Anim_.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        Anim_.HandleUpdate();

        if(rotateSpeed != 0)
        {
            newRotation = newRotation + rotateSpeed;
            gameObject.transform.rotation = Quaternion.Euler(0f,0f,newRotation);
        }
    }
}
