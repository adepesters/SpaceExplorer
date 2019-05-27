﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Slider slider;

    Player player;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        slider = GetComponent<Slider>();
        slider.maxValue = player.CurrentFuel;
        slider.value = player.MaxFuel;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        if (gameSession.SceneType == "space")
        {
            slider.enabled = true;
        }
        else if (gameSession.SceneType == "planet")
        {
            slider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.SceneType == "space")
        {
            slider.enabled = true;
            if (player)
            {
                slider.value = player.CurrentFuel;
            }
            else
            {
                slider.value = 0;
            }
        }
        else if (gameSession.SceneType == "planet")
        {
            slider.enabled = false;
        }
    }
}
