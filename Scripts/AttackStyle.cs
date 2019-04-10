using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStyle : MonoBehaviour
{
    public List<string> attackStyles;

    string[] defaultAttackStyles = new string[] { "lasers", "bombs" };

    int indexAttackStyle;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string attackStyle in defaultAttackStyles)
        {
            attackStyles.Add(attackStyle);
        }

        indexAttackStyle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PS4ControllerCheck>().IsL2Pressed())
        {
            ChangeToLeftAttackStyle();
        }

        if (FindObjectOfType<PS4ControllerCheck>().IsR2Pressed())
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
