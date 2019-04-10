using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningCanvas2 : MonoBehaviour
{
    bool currentWarning = false;
    bool pressed = false;
    bool bigGreenCircleAppear = false;
    bool transparentRedCircleAppear = false;

    GameObject redOpaqueCircle;
    GameObject redTransparentCircle;
    GameObject greenCircle;
    GameObject whiteCircle;

    Color off = new Color(1, 1, 1, 0);
    Color on = new Color(1, 1, 1, 1);

    float durationAvoidFeedback = 0.1f;
    float refractoryPeriod = 3f;

    float a = 0f;

    void Start()
    {
        redOpaqueCircle = GameObject.Find("redOpaqueCircleUI");
        redTransparentCircle = GameObject.Find("redTransparentCircleUI");
        greenCircle = GameObject.Find("greenCircleUI");
        whiteCircle = GameObject.Find("whiteCircleUI");

        greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        redOpaqueCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        redTransparentCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        whiteCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
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
            //greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //redOpaqueCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            //redTransparentCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            //whiteCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
        else if ((currentWarning == true || currentWarning == false) && bigGreenCircleAppear == true)
        {
            greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            redOpaqueCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            redTransparentCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            whiteCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
        else if ((currentWarning == true || currentWarning == false) && bigGreenCircleAppear == false && transparentRedCircleAppear == true)
        {
            greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            redOpaqueCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            redTransparentCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            whiteCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
        else if (currentWarning == false && bigGreenCircleAppear == false && transparentRedCircleAppear == false)
        {
            greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            redOpaqueCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            redTransparentCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            whiteCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
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

    //IEnumerator GrowRedCircle()
    //{
    //    while (true)
    //    {
    //        if (a >= 1f)
    //        {
    //            pressed = false;
    //            yield break;
    //        }
    //        Vector3 size = new Vector3(a, a, a);
    //        whiteCircle.gameObject.GetComponent<RectTransform>().localScale = size;
    //        redCircle.gameObject.GetComponent<RectTransform>().localScale = size;
    //        greenCircle.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    //        yield return new WaitForSeconds(0.01f);
    //        a += 0.01f * Time.deltaTime;
    //        a = Mathf.Clamp(a, 0f, 1f);
    //    }
    //}

    public void SetWarning(bool warning)
    {
        currentWarning = warning;
    }
}
