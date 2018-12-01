using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour
{
    int count;
    // Use this for initialization
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if(count>60)
        {
            Destroy(gameObject);
        }
    }
}
