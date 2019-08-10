using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResolution : MonoBehaviour
{

    void Start()
    {
        int width = 1920; //750; // or something else
        int height = 1080; // or something else
        bool isFullScreen = false; // should be windowed to run in arbitrary resolution
        int desiredFPS = 60; // or something else

        Screen.SetResolution(width, height, isFullScreen, desiredFPS);
    }

}
