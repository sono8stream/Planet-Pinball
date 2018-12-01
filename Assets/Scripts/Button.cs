using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
        Application.Quit();
    }

    public void Quit()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
        SceneManager.LoadScene(0);
    }
}
