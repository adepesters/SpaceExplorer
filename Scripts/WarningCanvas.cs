using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningCanvas : MonoBehaviour
{
    bool currentWarning = false;
    bool pressed = false;
    bool bigGreenCircleAppear = false;
    bool transparentRedCircleAppear = false;

    SpriteRenderer redCircle;
    SpriteRenderer greenCircle;
    SpriteRenderer bigGreenCircle;
    SpriteRenderer transparentRedCircle;

    Color off = new Color(1, 1, 1, 0);
    Color on = new Color(1, 1, 1, 1);

    float durationAvoidFeedback = 0.1f;
    float refractoryPeriod = 3f;

    void Start()
    {
        redCircle = GameObject.Find("avoid red").GetComponent<SpriteRenderer>();
        transparentRedCircle = GameObject.Find("avoid red transparent").GetComponent<SpriteRenderer>();
        greenCircle = GameObject.Find("avoid green").GetComponent<SpriteRenderer>();
        bigGreenCircle = GameObject.Find("avoid green big").GetComponent<SpriteRenderer>();

        redCircle.color = on;
        greenCircle.color = off;
        bigGreenCircle.color = off;
        transparentRedCircle.color = off;
    }

    void Update()
    {
        if (FindObjectOfType<ProximityDetector>().GetCollider() != null && currentWarning && pressed == false)
        {
            if (FindObjectOfType<PS4ControllerCheck>().IsL1Pressed() || FindObjectOfType<PS4ControllerCheck>().IsR1Pressed())
            {
                pressed = true;
                FindObjectOfType<ProximityDetector>().GetCollider().gameObject.GetComponent<Laser>().goesBackToEnemy = true;
                StartCoroutine(SuccessfulAvoidRoutine());
            }
        }

        if (currentWarning == true && bigGreenCircleAppear == false && transparentRedCircleAppear == false)
        {
            redCircle.color = off;
            greenCircle.color = off; //on;
            bigGreenCircle.color = off;
            transparentRedCircle.color = off;
        }
        else if ((currentWarning == true || currentWarning == false) && bigGreenCircleAppear == true)
        {
            redCircle.color = off;
            greenCircle.color = off;
            bigGreenCircle.color = off; //on;
            transparentRedCircle.color = off;
        }
        else if ((currentWarning == true || currentWarning == false) && bigGreenCircleAppear == false && transparentRedCircleAppear == true)
        {
            redCircle.color = off;
            greenCircle.color = off;
            bigGreenCircle.color = off;
            transparentRedCircle.color = off; // on;
        }
        else if (currentWarning == false && bigGreenCircleAppear == false && transparentRedCircleAppear == false)
        {
            redCircle.color = off; //on;
            greenCircle.color = off;
            bigGreenCircle.color = off;
            transparentRedCircle.color = off;
        }
    }

    IEnumerator SuccessfulAvoidRoutine()
    {
        bigGreenCircleAppear = true;
        yield return new WaitForSeconds(durationAvoidFeedback);
        bigGreenCircleAppear = false;
        transparentRedCircleAppear = true;
        yield return new WaitForSeconds(refractoryPeriod);
        transparentRedCircleAppear = false;
        pressed = false;
    }

    public void SetWarning(bool warning)
    {
        currentWarning = warning;
    }

}
