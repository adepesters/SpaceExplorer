using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Slider slider;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        slider = GetComponent<Slider>();
        slider.maxValue = player.CurrentFuel;
        slider.value = player.MaxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            slider.value = player.CurrentFuel;
        }
        else
        {
            slider.value = 0;
        }
    }
}
