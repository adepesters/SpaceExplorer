﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS4ControllerCheck : MonoBehaviour
{
    int counterL1Pressed;
    int counterL1Released;

    int counterR1Pressed;
    int counterR1Released;

    int durationThreshold = 20;

    bool continuousXPress = false;
    bool continuousL1Press = false;

    float currentPosVerticalStick = 0;
    float previousPosVerticalStick = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        previousPosVerticalStick = currentPosVerticalStick;
        currentPosVerticalStick = Input.GetAxis("Vertical");

        LongL1Press();
        LongR1Press();
    }

    public bool IsXPressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton1);
    }

    public bool IsXReleased()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton1);
    }

    public bool ContinuousXPress()
    {
        if (IsXPressed())
        {
            continuousXPress = true;
        }
        if (IsXReleased())
        {
            continuousXPress = false;
        }
        return continuousXPress;
    }

    public bool IsL1Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton4);
    }

    public bool IsL1Released()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton4);
    }

    public bool ContinuousL1Press()
    {
        if (IsL1Pressed())
        {
            continuousL1Press = true;
        }
        if (IsL1Released())
        {
            continuousL1Press = false;
        }
        return continuousL1Press;
    }

    public bool IsR1Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton5);
    }

    public bool IsL2Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton6);
    }

    public bool IsR2Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton7);
    }

    public bool LongL1Press()
    {
        bool isTrue = false;
        if (Input.GetKeyDown(KeyCode.JoystickButton4)) // L1
        {
            counterL1Pressed = Time.frameCount;
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton4)) // L1
        {
            counterL1Released = Time.frameCount;
        }
        if (counterL1Released - counterL1Pressed > durationThreshold)
        {
            counterL1Pressed = 0;
            counterL1Released = 0;
            isTrue = true;
        }
        return isTrue;
    }

    public bool LongR1Press()
    {
        bool isTrue = false;
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) // L1
        {
            counterR1Pressed = Time.frameCount;
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton5)) // L1
        {
            counterR1Released = Time.frameCount;
        }
        if (counterR1Released - counterR1Pressed > durationThreshold)
        {
            counterR1Pressed = 0;
            counterR1Released = 0;
            isTrue = true;
        }
        return isTrue;
    }

    public bool DiscreteMoveDown()
    {
        bool isTrue = false; ;
        if (Mathf.Abs(previousPosVerticalStick) < Mathf.Epsilon)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon && Input.GetAxis("Vertical") < 0)
            {
                isTrue = true;
            }
        }
        else
        {
            isTrue = false;
        }
        return isTrue;
    }

    public bool DiscreteMoveUp()
    {

        bool isTrue = false; ;
        if (Mathf.Abs(previousPosVerticalStick) < Mathf.Epsilon)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon && Input.GetAxis("Vertical") > 0)
            {
                isTrue = true;
            }
        }
        else
        {
            isTrue = false;
        }
        return isTrue;
    }
}