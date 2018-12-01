using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {

    GameObject sun;
    public static float coefficient = 0.00006f;
    Rigidbody rigidBody;
    GameObject explosion;
    public GameObject Explosion
    {
        get { return explosion; }
    }
    GameObject flash;
    bool isLooping = false;
    float adjRange = 1;
    bool passed = false;
    bool above = false;

    // Use this for initialization
    void Start()
    {
        sun = GameObject.Find("Sun");
        explosion = Instantiate(Resources.Load<GameObject>("Explosion"));
        explosion.SetActive(false);
        flash = Instantiate(Resources.Load<GameObject>("Flash"));
        flash.transform.SetParent(transform);
        flash.SetActive(false);
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
                GameController.score += (int)(100 * GameController.rate);
                GameController.UpdateScore();
                GameController.rate += 0.1f * transform.localScale.magnitude;
                GameController.UpdateRate(true);
                GameObject g = Instantiate(flash);
                g.SetActive(true);
                g.transform.SetParent(transform);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                g.GetComponent<EffectController>().enabled = true;
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
            /*Vector3 direction = sun.transform.position - transform.position;
            float distance = direction.magnitude;
            float p = Mathf.Sqrt(
                    coefficient * sun.GetComponent<Rigidbody>().mass / distance);
            if (rigidBody.velocity.magnitude < p + adjRange &&
                p - adjRange < rigidBody.velocity.magnitude)
            {
                GameController.score += 100;
                GameController.UpdateScore();
            }*/
        }
        else if (GameController.iniPosY + 2 < transform.position.y
            && transform.position.y < GameController.iniPosY + 3)
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
        //GameController.score -= (int)(200 * transform.localScale.magnitude);
        GameController.score -= 1000;
        if (GameController.score < 0)
        {
            GameController.score = 0;
        }
        GameController.UpdateScore(false);
        GameController.rate = 1;
        GameController.UpdateRate(false);
        GameController.SingExplode();
        explosion.SetActive(true);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
