using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

    public const string objectName = "SceneLoader";

    [SerializeField]
    int fadeFrames;
    float fadeSpeed;
    bool fadeIn, fadeOut;
    int sceneIndex;
    Waiter fadeWaiter;
    Image image;

    public Image FadeImage{ get { return image; } }

    // Use this for initialization
    void Awake()
    {
        gameObject.name = objectName;
        sceneIndex = -1;
        fadeSpeed = 1.0f / fadeFrames;
        fadeWaiter = new Waiter(fadeFrames);
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    public static LoadManager Find()
    {
        return GameObject.Find(objectName).GetComponent<LoadManager>();
    }

    public void LoadScene(int index)
    {
        if (index < 0 || SceneManager.sceneCountInBuildSettings < index)
        {
            Debug.Log(index);
            return;
        }
        StartCoroutine(LoadSceneCoroutine(index));
    }

    IEnumerator LoadSceneCoroutine(int index)
    {
        if (sceneIndex == index)
        {
            sceneIndex = -1;
        }
        Debug.Log("transit");
        image.enabled = true;
        while (FadeIn())
        {
            yield return new WaitForEndOfFrame();
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        yield return async;
        //yield return new WaitForSeconds(0.2f);

        while (FadeOut())
        {
            Debug.Log(image.color);
            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
    }

    bool FadeIn()
    {
        if (fadeWaiter.Wait())
        {
            fadeWaiter.Initialize();
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, 1);
            return false;
        }

        image.color += Color.black * fadeSpeed;
        return true;
    }

    bool FadeOut()
    {
        if (fadeWaiter.Wait())
        {
            fadeWaiter.Initialize();
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0);
            return false;
        }

        image.color -= Color.black * fadeSpeed;
        return true;
    }
}