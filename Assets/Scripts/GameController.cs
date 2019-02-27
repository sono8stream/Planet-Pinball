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
    [SerializeField]
    AudioClip explode, plus, shot;
    [SerializeField]
    GameObject scText, rtText;
    [SerializeField]
    GameObject explodeOrigin;
    [SerializeField]
    GameObject flashOrigin;
    [SerializeField]
    GameObject flashMinusOrigin;
    [SerializeField]
    float limitTime = 11;
    [SerializeField]
    Text tutorialText;
    int genNo = 0;
    public static int score = 0;
    float startTime = 0;
    float nowTime = 0;
    public float rate = 1;
    GameObject tarObj;
    Vector3 downPos;
    float iniPosX = 0;
    public float iniPosY = -6;
    float powScale = 1f;
    bool isHorizontal;
    SoundPlayer player;
    LoadManager loader;
    bool onEnd = false;
    bool onMoveLeft, onMoveRight;

    // Use this for initialization
    void Start()
    {
        score = 0;
        rate = 1;
        startTime = Time.time;
        SetPlanet();
        player = SoundPlayer.Find();
        loader = LoadManager.Find();
        UpdateTutorialMessage();
    }

    // Update is called once per frame
    void Update()
    {
        if (onEnd) return;

        if (CameraController.istouchingUI)
        {
            if (onMoveLeft) MoveLeft();
            if (onMoveRight) MoveRight();
        }
        else { 
            if (Input.GetMouseButtonDown(0))//スワイプしてplanetを生成、移動
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if (touchPos.y < -4)
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
                    if (Mathf.Abs(cPos.x) <= 4)
                    {
                        tarObj.transform.position 
                            = new Vector3(cPos.x, iniPosY, 0);
                        SetCursors(false, true);
                    }
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
                    UpdateTutorialMessage();
                }
                else
                {
                    float pow = (downPos.y
                        - Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                        * powScale;
                    if (pow > 1f)
                    {
                        tarObj.GetComponent<Rigidbody>().velocity = Vector2.up * pow;

                        GameObject g = Instantiate(explodeOrigin);
                        g.transform.position = tarObj.transform.position;
                        g.SetActive(true);            
                        player.PlaySE(shot);
                        SetPlanet();
                    }
                    SetCursors(true, true);
                    SetPowCursor(true, true);
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
        tarObj.GetComponent<Collider>().enabled = false;
        PlanetController planet = tarObj.GetComponent<PlanetController>();
        planet.controller = this;
        planet.explosion = explodeOrigin;
        planet.flash = flashOrigin;

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
        if (Mathf.Abs(vel - idealV) > idealV)
        {
            vel = vel < idealV ? 0.01f : idealV * 2;
        }
        float point = 2 - Mathf.Abs(vel - idealV) / idealV;
        SpriteRenderer s = cursors[2].GetComponent<SpriteRenderer>();
        s.enabled = true;
        cursors[2].transform.localScale = Vector3.one * point;
    }

    void UpdateTutorialMessage()
    {
        if (score > 0)
        {
            tutorialText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            tutorialText.transform.parent.gameObject.SetActive(true);
            if (Mathf.Abs(tarObj.transform.position.x) < 1)
            {
                tutorialText.text = "惑星をドラッグして位置を変えよう";
            }
            else
            {
                tutorialText.text = "上から下に引っ張って惑星を飛ばそう";
            }
        }
    }

    public void UpdateRate(bool up)
    {
        string t = "×" + rate.ToString("F2");
        rtText.GetComponent<Text>().text = t;
        /*
        GameObject g;
        if (up)
        {
            g = Instantiate(flashOrigin);
            g.transform.position = new Vector3(2f, 7.2f, 0);
        }
        else
        {
            g = Instantiate(flashMinusOrigin);
            g.transform.position = new Vector3(2f, 7.2f, 0);
        }
        */
    }

    void UpdateTime()
    {
        float leftTime = limitTime - Time.time + startTime;
        string t = ((int)leftTime).ToString();
        tmText.GetComponent<Text>().text = t.PadLeft(2, '0');
        if (leftTime < 0)
        {
            onEnd = true;
            Debug.Log("end");
            loader.LoadScene(2);
        }
    }

    public void UpdateScore(bool up = true)
    {
        string t = score.ToString();
        scText.GetComponent<Text>().text = t.PadLeft(6, '0');
        GameObject g;
        if (up)
        {
            g = Instantiate(flashOrigin);
            g.transform.position = new Vector3(2f, 6.2f, 0);
            player.PlaySE(plus);
        }
        else
        {
            g = Instantiate(flashMinusOrigin);
            g.transform.position = new Vector3(2f, 6.2f, 0);
        }
    }

    public void SingExplode()
    {
        player.PlaySE(explode);
    }

    public void SwitchMoveLeft(bool on)
    {
        onMoveLeft = on;
    }

    public void MoveLeft()
    {
        if (tarObj.transform.position.x < -4f) return;
        tarObj.transform.position += Vector3.left * 0.01f;
    }

    public void SwitchMoveRight(bool on)
    {
        onMoveRight = on;
    }

    public void MoveRight()
    {
        if (tarObj.transform.position.x >4f) return;
        tarObj.transform.position += Vector3.right * 0.01f;
    }
}
