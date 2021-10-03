using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {

    GameObject sun;
    public float coefficient = 0.00006f;
    public GameObject explosion;
    public GameObject flash;
    public GameController controller;
    Rigidbody rigidBody;
    bool isLooping = false;
    float adjRange = 1;
    bool passed = false;
    bool above = false;

    // Use this for initialization
    void Start()
    {
        sun = GameObject.Find("Sun");
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isLooping)
        {
            Vector3 direction = sun.transform.position - transform.position;
            float distance = direction.magnitude;
            distance *= distance;//距離の二乗
            float force = coefficient * sun.GetComponent<Rigidbody>().mass
                * rigidBody.mass / distance;//万有引力
            rigidBody.AddForce(direction/direction.magnitude * force,
                ForceMode.Force);
            if (!passed)
            {
                GameController.score += (int)(100 * controller.rate);
                controller.UpdateScore();
                controller.rate += 0.1f * transform.localScale.magnitude;
                controller.UpdateRate(true);
                /*
                GameObject g = Instantiate(flash);
                g.SetActive(true);
                g.transform.SetParent(transform);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = Vector3.one;
                */
                passed = true;
            }
            else if ((transform.position.y < sun.transform.position.y&&above)
                || (sun.transform.position.y < transform.position.y && !above))
            {
                passed = false;
                above = !above;
            }
        }
        else if (transform.position.y > sun.transform.position.y)
        {
            isLooping = true;
        }
        else if (controller.iniPosY + 2 < transform.position.y
            && transform.position.y < controller.iniPosY + 3)
        {
            GetComponent<Collider>().enabled = true;
        }
    }

    float CulculateForce()
    {
        Vector3 direction = sun.transform.position - transform.position;
        float distance = direction.magnitude;
        distance *= distance;//距離の二乗
        float force = coefficient * sun.GetComponent<Rigidbody>().mass
            * rigidBody.mass / distance;//万有引力
        return force;
    }
    
    public float CulculateVelocity()
    {
        if(Mathf.Abs(transform.position.x)<0.005f)
        {
            return 1;
        }
        float v = Mathf.Sqrt(coefficient * sun.GetComponent<Rigidbody>().mass
            / Mathf.Abs(transform.position.x));
        return v;
    }

    void OnCollisionEnter(Collision c)
    {
        //controller.score -= (int)(200 * transform.localScale.magnitude);
        GameController.score -= 1000;
        if (GameController.score < 0)
        {
            GameController.score = 0;
        }
        controller.UpdateScore(false);
        controller.rate = 1;
        controller.UpdateRate(false);
        controller.SingExplode();
        GameObject explodeEffect=Instantiate(explosion);
        explodeEffect.transform.position = transform.position;
        Destroy(gameObject);
    }
}
