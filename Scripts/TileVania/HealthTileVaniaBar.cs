using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTileVaniaBar : MonoBehaviour
{
    Slider slider;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        slider = GetComponent<Slider>();
        slider.maxValue = gameSession.MaxHealthPlanetPlayer;
        slider.value = gameSession.CurrentHealthPlanetPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = gameSession.CurrentHealthPlanetPlayer;
    }
}
