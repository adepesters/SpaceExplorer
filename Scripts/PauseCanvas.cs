using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] Text[] numberOfBonuses;

    void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        numberOfBonuses[0].text = FindObjectOfType<GameSession>().counterStarBronze.ToString();
        numberOfBonuses[1].text = FindObjectOfType<GameSession>().counterStarSilver.ToString();
        numberOfBonuses[2].text = FindObjectOfType<GameSession>().counterStarGold.ToString();
    }

    void Update()
    {
        numberOfBonuses[0].text = FindObjectOfType<GameSession>().counterStarBronze.ToString();
        numberOfBonuses[1].text = FindObjectOfType<GameSession>().counterStarSilver.ToString();
        numberOfBonuses[2].text = FindObjectOfType<GameSession>().counterStarGold.ToString();

        if (FindObjectOfType<PauseController>().IsGamePaused() && FindObjectOfType<PauseMenuController>().GetMenuPage() == "pause")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
