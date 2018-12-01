using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

    [SerializeField]
    GameObject[] planets;
    [SerializeField]
    GameObject tmText;
    [SerializeField]
    GameObject[] cursors;
    static AudioClip explode, plus, shot;
    static GameObject scText, rtText;
    static GameObject flash;
    static GameObject flashMi;
    int genNo = 0;
    static string savePath;
    public static bool isHighScore;
    public static int score = 0;
    float limitTime = 63;
    float startTime = 0;
    float nowTime = 0;
    public static float rate = 1;
    GameObject tarObj;
    Vector3 downPos;
    float iniPosX = 0;
    public static float iniPosY = -6;
    float powScale = 1f;
    bool isHorizontal;
    static AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        score = 0;
        rate = 1;
        isHighScore = false;
        savePath = "ScoreData";
        explode = Resources.Load<AudioClip>("bomb1");
        plus = Resources.Load<AudioClip>("plus3");
        shot = Resources.Load<AudioClip>("shot1");
        scText = GameObject.Find("Canvas").transform.FindChild("Score").gameObject;
        rtText = GameObject.Find("Canvas").transform.FindChild("Rate").gameObject;
        flash = Instantiate(Resources.Load<GameObject>("ScorePlus"));
        flash.GetComponent<EffectController>().enabled = false;
        flashMi = Instantiate(Resources.Load<GameObject>("ScoreMinus"));
        flashMi.GetComponent<EffectController>().enabled = false;
        startTime = Time.time;
        SetPlanet();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraController.istouchingUI)
        {
            if (Input.GetMouseButtonDown(0))//スワイプしてplanetを生成、移動
            {
                /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))*/
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (touchPos.y < -5)
                {
                    isHorizontal = true;
                    SetCursors(false, true);
                    SetPowCursor(false, false);
                    tarObj.transform.position = new Vector3(touchPos.x, iniPosY, 0);
                }
                else
                {
                    downPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    isHorizontal = false;
                    SetCursors(false, false);
                    SetPowCursor(false, true);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (isHorizontal)
                {
                    Vector3 cPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tarObj.transform.position = new Vector3(cPos.x, iniPosY, 0);
                    SetCursors(false, true);
                }
                else
                {
                    UpdatePowCursor();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isHorizontal)
                {
                    iniPosX = tarObj.transform.position.x;
                    isHorizontal = false;
                    SetCursors(true, true);
                    SetPowCursor(true, true);
                }
                else
                {
                    float pow = (downPos.y
                        - Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                        * powScale;
                    if (Mathf.Abs(pow) > 1f)
                    {
                        tarObj.GetComponent<Rigidbody>().velocity = Vector2.up * pow;
                        tarObj.GetComponent<Collider>().enabled = false;
                        GameObject g = Instantiate(tarObj.GetComponent<PlanetController>().Explosion);
                        g.transform.position = tarObj.transform.position;
                        g.SetActive(true);                        
                        audioSource.PlayOneShot(shot);
                        SetPlanet();
                    }
                }
            }
        }
        UpdateTime();
    }

    void SetPlanet()//planetを画面上にセット
    {
        tarObj = Instantiate(planets[genNo]);
        tarObj.transform.position = new Vector2(iniPosX, iniPosY);
        tarObj.transform.SetParent(GameObject.Find("Planets").transform);
        genNo = Random.Range(0, planets.GetLength(0));
        Debug.Log(genNo);
        SetCursors(true,true);
        SetPowCursor(true,true);
    }

    void SetCursors(bool flash,bool enable)
    {
        if (enable)
        {
            cursors[0].GetComponent<SpriteRenderer>().enabled = true;
            cursors[1].GetComponent<SpriteRenderer>().enabled = true;
            cursors[0].transform.position
                = tarObj.transform.position + Vector3.right;
            cursors[1].transform.position
                = tarObj.transform.position + Vector3.left;
            if (flash)
            {
                cursors[0].GetComponent<Animator>().SetBool("isSelecting", false);
                cursors[1].GetComponent<Animator>().SetBool("isSelecting", false);
            }
            else
            {
                cursors[0].GetComponent<Animator>().SetBool("isSelecting", true);
                cursors[1].GetComponent<Animator>().SetBool("isSelecting", true);
            }
        }
        else
        {
            cursors[0].GetComponent<SpriteRenderer>().enabled = false;
            cursors[1].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void SetPowCursor(bool flash, bool enable)
    {
        if (enable)
        {
            cursors[2].transform.localScale = Vector3.one;
            cursors[2].transform.position
                = tarObj.transform.position + Vector3.up * 3;
            cursors[2].GetComponent<SpriteRenderer>().enabled = true;
            if (flash)
            {
                cursors[2].GetComponent<Animator>().SetBool("isSelecting", false);
            }
            else
            {
                cursors[2].GetComponent<Animator>().SetBool("isSelecting", true);
            }
        }
        else
        {
            cursors[2].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void UpdatePowCursor()
    {
        float idealV
            = tarObj.GetComponent<PlanetController>().CulculateVelocity();
        if (idealV < 0.05f)
        {
            return;
        }
        float vel = (downPos.y
                - Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                * powScale;
        float point = 2 - Mathf.Abs(vel - idealV) / idealV;
        SpriteRenderer s = cursors[2].GetComponent<SpriteRenderer>();
        s.enabled = true;
        Debug.Log(idealV);
        Debug.Log(point);
        cursors[2].transform.localScale = Vector3.one * point;
    }

    public static void UpdateRate(bool up)
    {
        string t = "×" + rate.ToString("F2");
        rtText.GetComponent<Text>().text = t;
        GameObject g;
        if (up)
        {
            g = Instantiate(flash);
            g.transform.position = new Vector3(-4f, 4.2f, 0);
        }
        else
        {
            g = Instantiate(flashMi);
            g.transform.position = new Vector3(-0.5f, 4.2f, 0);
        }
        g.GetComponent<EffectController>().enabled = true;
    }

    void UpdateTime()
    {
        float leftTime = limitTime - Time.time + startTime;
        string t = ((int)leftTime).ToString();
        tmText.GetComponent<Text>().text = t.PadLeft(2, '0');
        if (leftTime < 0)
        {
            if (score > SaveLoadScore(false))
            {
                SaveLoadScore(true);
                isHighScore = true;
            }
            SceneManager.LoadScene(2);
        }
    }

    public static void UpdateScore(bool up = true)
    {
        string t = score.ToString();
        scText.GetComponent<Text>().text = t.PadLeft(6, '0');
        GameObject g;
        if (up)
        {
            g = Instantiate(flash);
            audioSource.PlayOneShot(plus);
        }
        else
        {
            g = Instantiate(flashMi);
        }
        g.GetComponent<EffectController>().enabled = true;
    }

    public static void SingExplode()
    {
        audioSource.PlayOneShot(explode);
    }

    public static int SaveLoadScore(bool save)
    {
        int sc = 0;
        if(save)
        {
            PlayerPrefs.SetInt(savePath, score);
        }
        else
        {
            sc = PlayerPrefs.GetInt(savePath);
        }
        return sc;
    }
}
