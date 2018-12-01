using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Waiter w;
    Text t;

    // Use this for initialization
    void Start()
    {
        w = new Waiter(10);
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (w.Wait()) { w.Initialize(); }
        t.color += Color.white
            * 0.02f * Mathf.Sin(2 * Mathf.PI * w.Count / w.Limit);
    }

    public void UpdatePeriod(int nextPeriod)
    {
        w.Initialize(nextPeriod);
    }
}
