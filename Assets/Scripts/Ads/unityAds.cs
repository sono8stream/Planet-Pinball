using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class unityAds : MonoBehaviour {

    [SerializeField]
    Button buttonScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAd()
    {
        buttonScript.PlayEnterSE();
        string adId = "rewardedVideo";
        if (Advertisement.IsReady(adId))
        {
            Debug.Log("Play ad");
            ShowOptions options 
                = new ShowOptions { resultCallback = ShowAdResult };
            Advertisement.Show(adId, options);
        }
    }

    void ShowAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                buttonScript.UpdateLife(10);
                buttonScript.CloseRecoverWindow();
                break;
        }
    }
}
