﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;

    Player player;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        slider = GetComponent<Slider>();
        slider.maxValue = player.GetHealthPlayer();
        slider.value = slider.maxValue;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            slider.value = player.GetHealthPlayer();
        }
        else
        {
            slider.value = 0;
        }
    }
}
