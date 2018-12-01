using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

    [SerializeField]
    AudioClip se;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
        transform.Find("Play").Find("Text")
            .GetComponent<ButtonScript>().UpdatePeriod(3);
        LoadManager.Find().LoadScene(1);
    }

    public void Exit()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
        transform.Find("Exit").Find("Text")
            .GetComponent<ButtonScript>().UpdatePeriod(3);
        Application.Quit();
    }

    public void Quit()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
        LoadManager.Find().LoadScene(0);
    }
}
