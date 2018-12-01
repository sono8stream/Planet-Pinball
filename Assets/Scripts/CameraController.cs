using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    Camera camera;
    public static bool istouchingUI;

    int zPhase = 0;
    const int ZLIMIT = 1;
    bool isZooming = true;

    // Use this for initialization
    void Start()
    {
        istouchingUI = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Zoom()
    {
        zPhase += isZooming ? 1 : -1;
        if (zPhase == ZLIMIT || zPhase == -ZLIMIT)//拡大縮小限界で反転
        {
            isZooming = !isZooming;
        }
        camera.orthographicSize = 8 - zPhase * 4;
    }

    public void CheckUI(bool touch)
    {
        istouchingUI = touch;
    }
}
