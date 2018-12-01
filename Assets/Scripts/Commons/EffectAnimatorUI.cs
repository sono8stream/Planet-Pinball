using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimatorUI : MonoBehaviour
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

    Image image;

    // Use this for initialization
    void Start()
    {
        animCounter = new Counter(spriteFrames.Length);
        frameCounter = new Counter(frameCntPerFrame);
        loopCounter = new Counter(loopCnt);

        image = GetComponent<Image>();
        image.sprite = spriteFrames[0];
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
                    image.enabled = false;
                    Destroy(gameObject);
                }
            }
            image.sprite = spriteFrames[animCounter.Now];
        }
    }
}