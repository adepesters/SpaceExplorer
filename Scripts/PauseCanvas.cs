using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] Text[] numberOfBonuses;

    GameSession gameSession;
    PauseController pauseController;
    PauseMenuController pauseMenuController;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        pauseController = FindObjectOfType<PauseController>();
        pauseMenuController = FindObjectOfType<PauseMenuController>();

        gameObject.GetComponent<Canvas>().enabled = false;
        numberOfBonuses[0].text = gameSession.counterStarBronze.ToString();
        numberOfBonuses[1].text = gameSession.counterStarSilver.ToString();
        numberOfBonuses[2].text = gameSession.counterStarGold.ToString();
    }

    void Update()
    {
        numberOfBonuses[0].text = gameSession.counterStarBronze.ToString();
        numberOfBonuses[1].text = gameSession.counterStarSilver.ToString();
        numberOfBonuses[2].text = gameSession.counterStarGold.ToString();

        if (pauseController.IsGamePaused() && pauseMenuController.GetMenuPage() == "pause")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
