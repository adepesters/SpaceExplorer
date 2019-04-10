using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = FindObjectOfType<Player>().GetHealthPlayer();
        slider.value = slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Player>())
        {
            slider.value = FindObjectOfType<Player>().GetHealthPlayer();
        }
        else
        {
            slider.value = 0;
        }
    }
}
