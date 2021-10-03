using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Result : MonoBehaviour
{

    [SerializeField]
    GameObject text;

    // Use this for initialization
    void Start()
    {
        string t;
        int highScore = SaveLoadScore(false);
        
        if (highScore<GameController.score)
        {
            t = "High Score !!\n        " + GameController.score.ToString();
            SaveLoadScore(true);
        }
        else
        {
            t = "Score:\n        " + GameController.score.ToString()
                + "\n High Score:\n        "
                + highScore.ToString();
        }
        text.GetComponent<Text>().text = t;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int SaveLoadScore(bool save)
    {
        int sc = 0;
        string savePath = "ScoreData";
        if (save)
        {
            PlayerPrefs.SetInt(savePath, GameController.score);
        }
        else
        {
            sc = PlayerPrefs.GetInt(savePath);
        }
        return sc;
    }
}
