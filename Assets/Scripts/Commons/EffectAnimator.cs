using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite[] spriteFrames;
    [SerializeField]
    int frameCntPerFrame;
    [SerializeField]
    int loopCnt;//-1なら無限ループ

    int frameWidth;
    int frameHeight;

    Counter animCounter;
    Counter frameCounter;
    Counter loopCounter;

    SpriteRenderer renderer;

    // Use this for initialization
    void Start()
    {
        animCounter = new Counter(spriteFrames.Length);
        frameCounter = new Counter(frameCntPerFrame);
        loopCounter = new Counter(loopCnt);

        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = spriteFrames[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCounter.Count())
        {
            frameCounter.Initialize();
            if (animCounter.Count())
            {
                animCounter.Initialize();
                if (loopCnt > 0 && loopCounter.Count())
                {
                    renderer.enabled = false;
                    Destroy(gameObject);
                }
            }
            renderer.sprite = spriteFrames[animCounter.Now];
        }
    }
}