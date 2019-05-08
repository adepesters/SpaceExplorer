using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStyle : MonoBehaviour
{
    public List<string> attackStyles;

    string[] defaultAttackStyles = new string[] { "lasers", "bombs" };

    int indexAttackStyle;

    PS4ControllerCheck PS4ControllerCheck;

    // Start is called before the first frame update
    void Start()
    {
        PS4ControllerCheck = FindObjectOfType<PS4ControllerCheck>();

        foreach (string attackStyle in defaultAttackStyles)
        {
            attackStyles.Add(attackStyle);
        }

        indexAttackStyle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PS4ControllerCheck.IsL2Pressed())
        {
            ChangeToLeftAttackStyle();
        }

        if (PS4ControllerCheck.IsR2Pressed())
        {
            ChangeToRightAttackStyle();
        }
    }

    private void ChangeToRightAttackStyle()
    {
        indexAttackStyle++;
        if (indexAttackStyle > attackStyles.Count - 1)
        {
            indexAttackStyle = 0;
        }
    }

    private void ChangeToLeftAttackStyle()
    {
        indexAttackStyle--;
        if (indexAttackStyle < 0)
        {
            indexAttackStyle = attackStyles.Count - 1;
        }
    }

    public string GetAttackStyle()
    {
        return attackStyles[indexAttackStyle];
    }

}
