using System;
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
    bool continuousL2Press = false;
    bool continuousR2Press = false;
    bool continuousR1Press = false;

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

        ContinuousL2Press();
        ContinuousL1Press();
        ContinuousXPress();
        ContinuousR1Press();
        ContinuousR2Press();
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

    public bool ContinuousL2Press()
    {
        if (IsL2Pressed())
        {
            continuousL2Press = true;
        }
        if (IsL2Released())
        {
            continuousL2Press = false;
        }
        return continuousL2Press;
    }

    public bool ContinuousR1Press()
    {
        if (IsR1Pressed())
        {
            continuousR1Press = true;
        }
        if (IsR1Released())
        {
            continuousR1Press = false;
        }
        return continuousR1Press;
    }

    public bool ContinuousR2Press()
    {
        if (IsR2Pressed())
        {
            continuousR2Press = true;
        }
        if (IsR2Released())
        {
            continuousR2Press = false;
        }
        return continuousR2Press;
    }

    public bool IsR1Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton5);
    }

    public bool IsR1Released()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton5);
    }

    public bool IsL2Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton6);
    }

    public bool IsL2Released()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton6);
    }

    public bool IsR2Pressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton7);
    }

    public bool IsR2Released()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton7);
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

    public bool IsSquarePressed()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool IsSquareReleased()
    {
        return Input.GetKeyUp(KeyCode.JoystickButton0);
    }

    public bool noButtonPressed()
    {
        return !(IsSquarePressed() || IsSquareReleased() || continuousR1Press || continuousL1Press || IsR2Pressed() || IsR2Released() ||
        IsR1Pressed() || IsR1Released() || IsL1Pressed() || IsL1Released() || IsL2Pressed() || IsL2Released() ||
            continuousR2Press || continuousL2Press || continuousXPress || IsXPressed() || IsXReleased() ||
        (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon || Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon));
    }
}
