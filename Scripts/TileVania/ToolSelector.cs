using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    public List<string> tools;

    string[] defaultTools = new string[] { "sword", "grappin" };

    int indexTool;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string tool in defaultTools)
        {
            tools.Add(tool);
        }

        indexTool = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PS4ControllerCheck>().IsL2Pressed())
        {
            ChangeToLeftTool();
        }

        if (FindObjectOfType<PS4ControllerCheck>().IsR2Pressed())
        {
            ChangeToRightTool();
        }
    }

    private void ChangeToRightTool()
    {
        indexTool++;
        if (indexTool > tools.Count - 1)
        {
            indexTool = 0;
        }
    }

    private void ChangeToLeftTool()
    {
        indexTool--;
        if (indexTool < 0)
        {
            indexTool = tools.Count - 1;
        }
    }

    public string GetTool()
    {
        return tools[indexTool];
    }
}
