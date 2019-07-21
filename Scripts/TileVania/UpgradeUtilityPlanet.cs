using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUtilityPlanet : MonoBehaviour
{
    // upgrade text
    [SerializeField] Text upgradeHealthText;
    [SerializeField] Text upgradeStrengthText;
    [SerializeField] Text healText;
    [SerializeField] Image upgradeHealthArrow;
    [SerializeField] Image upgradeStrengthArrow;
    [SerializeField] Image healArrow;

    // upgrade numbers
    [SerializeField] Text numbersHealth;
    [SerializeField] Text numberStrength;
    [SerializeField] Text availablePixelBlood;

    Color selected = new Color(1f, 0.9897198f, 0.4481132f, 1);
    Color nonselected = new Color(1f, 0.9897198f, 0.4481132f, 0.4f);
    Color clicked = new Color(0.6784032f, 1, 0.3632075f, 1);

    PauseController pauseController;
    PS4ControllerCheck PS4ControllerCheck;
    GameSession gameSession;

    string selectedButton = "heal";

    float timeBetweenUpgrade = 0.1f;
    float counterUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        pauseController = FindObjectOfType<PauseController>();
        PS4ControllerCheck = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        healText.GetComponent<Text>().color = selected;
        healArrow.GetComponent<Image>().color = selected;
        upgradeHealthText.GetComponent<Text>().color = nonselected;
        upgradeHealthArrow.GetComponent<Image>().color = nonselected;
        upgradeStrengthText.GetComponent<Text>().color = nonselected;
        upgradeStrengthArrow.GetComponent<Image>().color = nonselected;

        counterUpgrade = timeBetweenUpgrade;
    }

    // Update is called once per frame
    void Update()
    {
        availablePixelBlood.GetComponent<Text>().text = gameSession.CounterPixelBlood.ToString();
        numbersHealth.GetComponent<Text>().text = gameSession.CurrentHealthPlanetPlayer + " / " + gameSession.MaxHealthPlanetPlayer;
        numberStrength.GetComponent<Text>().text = "Att. " + gameSession.SwordDamage;

        if (pauseController.IsGamePaused())
        {
            counterUpgrade -= Time.fixedDeltaTime;

            if (PS4ControllerCheck.DiscreteMoveUp() && selectedButton == "strengthUpgrade")
            {
                selectedButton = "healthUpgrade";
                upgradeHealthText.GetComponent<Text>().color = selected;
                upgradeHealthArrow.GetComponent<Image>().color = selected;
                upgradeStrengthText.GetComponent<Text>().color = nonselected;
                upgradeStrengthArrow.GetComponent<Image>().color = nonselected;
                healText.GetComponent<Text>().color = nonselected;
                healArrow.GetComponent<Image>().color = nonselected;
            }
            else if (PS4ControllerCheck.DiscreteMoveDown())
            {
                selectedButton = "strengthUpgrade";
                upgradeHealthText.GetComponent<Text>().color = nonselected;
                upgradeHealthArrow.GetComponent<Image>().color = nonselected;
                upgradeStrengthText.GetComponent<Text>().color = selected;
                upgradeStrengthArrow.GetComponent<Image>().color = selected;
                healText.GetComponent<Text>().color = nonselected;
                healArrow.GetComponent<Image>().color = nonselected;
            }
            if (PS4ControllerCheck.DiscreteMoveRight() && selectedButton == "heal")
            {
                selectedButton = "healthUpgrade";
                upgradeHealthText.GetComponent<Text>().color = selected;
                upgradeHealthArrow.GetComponent<Image>().color = selected;
                upgradeStrengthText.GetComponent<Text>().color = nonselected;
                upgradeStrengthArrow.GetComponent<Image>().color = nonselected;
                healText.GetComponent<Text>().color = nonselected;
                healArrow.GetComponent<Image>().color = nonselected;
            }
            if (PS4ControllerCheck.DiscreteMoveLeft() && (selectedButton == "healthUpgrade" || selectedButton == "strengthUpgrade"))
            {
                selectedButton = "heal";
                healText.GetComponent<Text>().color = selected;
                healArrow.GetComponent<Image>().color = selected;
                upgradeHealthText.GetComponent<Text>().color = nonselected;
                upgradeHealthArrow.GetComponent<Image>().color = nonselected;
                upgradeStrengthText.GetComponent<Text>().color = nonselected;
                upgradeStrengthArrow.GetComponent<Image>().color = nonselected;
            }

            if (selectedButton == "healthUpgrade")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    upgradeHealthText.GetComponent<Text>().color = clicked;
                    upgradeHealthArrow.GetComponent<Image>().color = clicked;
                    if (counterUpgrade < 0f && gameSession.CounterPixelBlood > 0)
                    {
                        gameSession.CounterPixelBlood--;
                        gameSession.MaxHealthPlanetPlayer++;
                        counterUpgrade = timeBetweenUpgrade;
                        timeBetweenUpgrade -= 0.002f;
                        timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                    }
                }
                else
                {
                    upgradeHealthText.GetComponent<Text>().color = selected;
                    upgradeHealthArrow.GetComponent<Image>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "strengthUpgrade")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    upgradeStrengthText.GetComponent<Text>().color = clicked;
                    upgradeStrengthArrow.GetComponent<Image>().color = clicked;
                    if (counterUpgrade < 0f && gameSession.CounterPixelBlood > 0)
                    {
                        gameSession.CounterPixelBlood--;
                        gameSession.SwordDamage++;
                        counterUpgrade = timeBetweenUpgrade;
                        timeBetweenUpgrade -= 0.002f;
                        timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                    }
                }
                else
                {
                    upgradeStrengthText.GetComponent<Text>().color = selected;
                    upgradeStrengthArrow.GetComponent<Image>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "heal")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    if (gameSession.CurrentHealthPlanetPlayer != gameSession.MaxHealthPlanetPlayer)
                    {
                        healText.GetComponent<Text>().color = clicked;
                        healArrow.GetComponent<Image>().color = clicked;
                        if (counterUpgrade < 0f && gameSession.CounterPixelBlood > 0)
                        {
                            gameSession.CounterPixelBlood--;
                            gameSession.CurrentHealthPlanetPlayer++;
                            counterUpgrade = timeBetweenUpgrade;
                            timeBetweenUpgrade -= 0.002f;
                            timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                        }
                    }
                }
                else
                {
                    healText.GetComponent<Text>().color = selected;
                    healArrow.GetComponent<Image>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
        }
    }

}
