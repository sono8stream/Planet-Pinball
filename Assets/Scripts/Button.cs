using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Button : MonoBehaviour {
    
    public AudioClip se;
    [SerializeField]
    bool isTitle;
    [SerializeField]
    GameObject lifeRecoverWindow;
    [SerializeField]
    Text lifeCountText;

    public int life;

    SoundPlayer player;
    LoadManager loader;
    string savePath;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    // Use this for initialization
    void Start () {
        player = SoundPlayer.Find();
        loader = LoadManager.Find();
        savePath = "LifeData";
        LoadLife();
        if (isTitle)
        {
            UpdateLife(life);
        }
        else
        {
            UpdateLife(life - 1);
        }
    }

    public void Play()
    {
        PlayEnterSE();
        if (life > 0)
        {
            loader.LoadScene(1);
        }
        else
        {
            lifeRecoverWindow.SetActive(true);
        }
    }

    public void Exit()
    {
        PlayEnterSE();
        Application.Quit();
    }

    public void Quit()
    {
        PlayEnterSE();
        loader.LoadScene(0);
    }

    void SaveLife()
    {
        PlayerPrefs.SetInt(savePath, life);
    }

    void LoadLife()
    {
        if (PlayerPrefs.HasKey(savePath))
        {
            life = PlayerPrefs.GetInt(savePath);
        }
        else
        {
            life = 5;
            PlayerPrefs.SetInt(savePath, life);
        }
    }

    public void UpdateLife(int nextLife)
    {
        life = nextLife;
        SaveLife();
        lifeCountText.text = "×" + life.ToString();
    }

    public void CloseRecoverWindow()
    {
        lifeRecoverWindow.SetActive(false);
        player.PlaySE(se);
    }

    public void PlayEnterSE()
    {
        player.PlaySE(se);
    }
}
