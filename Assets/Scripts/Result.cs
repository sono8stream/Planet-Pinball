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
        if (GameController.isHighScore)
        {
            t = "High Score !!\n        " + GameController.score.ToString();
        }
        else
        {
            t = "Score:\n        " + GameController.score.ToString()
                + "\n High Score:\n        "
                + GameController.SaveLoadScore(false).ToString();
        }
        text.GetComponent<Text>().text = t;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
