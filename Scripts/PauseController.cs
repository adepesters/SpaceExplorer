using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            isPaused = !isPaused;
            FindObjectOfType<PauseMenuController>().SetIndexMenuPage(0);
        }

        if (isPaused)
        {
            Time.timeScale = 0;
            GameObject.Find("Background Pause Menu").GetComponent<Canvas>().enabled = true; // I added this background to prevent some clipping 
                                                                                            // when we go from one page of the menu to the next
        }
        else
        {
            Time.timeScale = 1;
            GameObject.Find("Background Pause Menu").GetComponent<Canvas>().enabled = false;
        }
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}
