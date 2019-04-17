using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PauseController>().IsGamePaused() && FindObjectOfType<PauseMenuController>().GetMenuPage() == "world map")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
