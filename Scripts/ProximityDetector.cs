using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    GameObject detectedCollider;

    bool warning = false;

    float durationWarning = 0.5f;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(warning);
        //Debug.Log(detectedCollider != null);
        //Debug.Log(detectedCollider.gameObject != null);
        //Debug.Log(pressed);
        //Debug.Log(transparentRedCircleAppear);

        Vector2 pos = new Vector2(player.transform.position.x, player.transform.position.y);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Laser Enemy"))
        {
            detectedCollider = collision.gameObject;
            StartCoroutine(Warning());
        }
    }

    IEnumerator Warning()
    {
        //warning = true;
        FindObjectOfType<WarningCanvas2>().SetWarning(true);
        yield return new WaitForSeconds(durationWarning);
        //warning = false;
        FindObjectOfType<WarningCanvas2>().SetWarning(false);
    }

    public GameObject GetCollider()
    {
        return detectedCollider;
    }


    //GameObject detectedCollider;

    //bool warning = false;
    //bool pressed = false;
    //bool bigGreenCircleAppear = false;
    //bool transparentRedCircleAppear = false;

    //SpriteRenderer redCircle;
    //SpriteRenderer greenCircle;
    //SpriteRenderer bigGreenCircle;
    //SpriteRenderer transparentRedCircle;

    //Color off = new Color(1, 1, 1, 0);
    //Color on = new Color(1, 1, 1, 1);

    //float durationWarning = 0.5f;
    //float durationAvoidFeedback = 0.1f;
    //float refractoryPeriod = 3f;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    redCircle = GameObject.Find("avoid red").GetComponent<SpriteRenderer>();
    //    transparentRedCircle = GameObject.Find("avoid red transparent").GetComponent<SpriteRenderer>();
    //    greenCircle = GameObject.Find("avoid green").GetComponent<SpriteRenderer>();
    //    bigGreenCircle = GameObject.Find("avoid green big").GetComponent<SpriteRenderer>();

    //    redCircle.color = on;
    //    greenCircle.color = off;
    //    bigGreenCircle.color = off;
    //    transparentRedCircle.color = off;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //Debug.Log(warning);
    //    //Debug.Log(detectedCollider != null);
    //    //Debug.Log(detectedCollider.gameObject != null);
    //    //Debug.Log(pressed);
    //    //Debug.Log(transparentRedCircleAppear);

    //    Vector2 pos = new Vector2(FindObjectOfType<Player>().transform.position.x, FindObjectOfType<Player>().transform.position.y);
    //    transform.position = pos;

    //    if (detectedCollider != null && warning && pressed == false)
    //    {
    //        if (FindObjectOfType<PS4ControllerCheck>().IsL1Pressed() || FindObjectOfType<PS4ControllerCheck>().IsR1Pressed())
    //        {
    //            pressed = true;
    //            detectedCollider.gameObject.GetComponent<Laser>().goesBackToEnemy = true;
    //            StartCoroutine(SuccessfulAvoidRoutine());
    //        }
    //    }

    //    if (warning == true && bigGreenCircleAppear == false && transparentRedCircleAppear == false)
    //    {
    //        redCircle.color = off;
    //        greenCircle.color = off; //on;
    //        bigGreenCircle.color = off;
    //        transparentRedCircle.color = off;
    //    }
    //    else if ((warning == true || warning == false) && bigGreenCircleAppear == true)
    //    {
    //        redCircle.color = off;
    //        greenCircle.color = off;
    //        bigGreenCircle.color = off; //on;
    //        transparentRedCircle.color = off;
    //    }
    //    else if ((warning == true || warning == false) && bigGreenCircleAppear == false && transparentRedCircleAppear == true)
    //    {
    //        redCircle.color = off;
    //        greenCircle.color = off;
    //        bigGreenCircle.color = off;
    //        transparentRedCircle.color = off; // on;
    //    }
    //    else if (warning == false && bigGreenCircleAppear == false && transparentRedCircleAppear == false)
    //    {
    //        redCircle.color = off; //on;
    //        greenCircle.color = off;
    //        bigGreenCircle.color = off;
    //        transparentRedCircle.color = off;
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Contains("Laser Enemy"))
    //    {
    //        detectedCollider = collision.gameObject;
    //        StartCoroutine(Warning());
    //    }
    //}

    //IEnumerator Warning()
    //{
    //    warning = true;
    //    yield return new WaitForSeconds(durationWarning);
    //    warning = false;
    //}

    //IEnumerator SuccessfulAvoidRoutine()
    //{
    //    bigGreenCircleAppear = true;
    //    yield return new WaitForSeconds(durationAvoidFeedback);
    //    bigGreenCircleAppear = false;
    //    transparentRedCircleAppear = true;
    //    yield return new WaitForSeconds(refractoryPeriod);
    //    transparentRedCircleAppear = false;
    //    pressed = false;
    //}



}
