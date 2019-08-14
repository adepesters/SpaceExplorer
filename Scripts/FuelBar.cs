using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Slider slider;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        slider = GetComponent<Slider>();
        slider.maxValue = gameSession.MaxFuelSpacePlayer;
        slider.value = gameSession.CurrentFuelSpacePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = gameSession.MaxFuelSpacePlayer;
        slider.value = gameSession.CurrentFuelSpacePlayer;
    }
}
