using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    GameSession gameSession;
    RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        if (gameSession.SceneType == "space")
        {
            rawImage.enabled = true;
        }
        else if (gameSession.SceneType == "planet")
        {
            rawImage.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.SceneType == "space")
        {
            rawImage.enabled = true;
        }
        else if (gameSession.SceneType == "planet")
        {
            rawImage.enabled = false;
        }
    }
}
