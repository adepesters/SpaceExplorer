using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSliderFill : MonoBehaviour
{
    GameSession gameSession;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        if (gameSession.SceneType == "space")
        {
            image.enabled = true;
        }
        else if (gameSession.SceneType == "planet")
        {
            image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.SceneType == "space")
        {
            image.enabled = true;
        }
        else if (gameSession.SceneType == "planet")
        {
            image.enabled = false;
        }
    }
}
