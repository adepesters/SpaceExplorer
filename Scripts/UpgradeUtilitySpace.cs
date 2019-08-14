using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUtilitySpace : MonoBehaviour
{
    // upgrade text
    [SerializeField] Text upgradeShieldText;
    [SerializeField] Text upgradeFuelText;
    [SerializeField] Text upgradeAttackText;
    [SerializeField] Text restoreShieldText;
    [SerializeField] Text restoreFuelText;
    [SerializeField] Image upgradeShieldArrow;
    [SerializeField] Image upgradeFuelArrow;
    [SerializeField] Image upgradeAttackArrow;
    [SerializeField] Image restoreShieldArrow;
    [SerializeField] Image restoreFuelArrow;
    [SerializeField] Text upgradeShieldPriceText;
    [SerializeField] Text upgradeFuelPriceText;
    [SerializeField] Text upgradeAttackPriceText;
    [SerializeField] Text restoreShieldPriceText;
    [SerializeField] Text restoreFuelPriceText;

    // upgrade numbers
    [SerializeField] Text numbersShield;
    [SerializeField] Text numbersFuel;
    [SerializeField] Text numberAttack;
    [SerializeField] Text availablePixelBlood;

    Color selected = new Color(1f, 0.9897198f, 0.4481132f, 1);
    Color nonselected = new Color(1f, 0.9897198f, 0.4481132f, 0.4f);
    Color clicked = new Color(0.6784032f, 1, 0.3632075f, 1);

    PauseController pauseController;
    PS4ControllerCheck PS4ControllerCheck;
    GameSession gameSession;

    string selectedButton = "restoreShield";

    float timeBetweenUpgrade = 0.1f;
    float counterUpgrade;

    float counterMove;

    // Start is called before the first frame update
    void Start()
    {
        pauseController = FindObjectOfType<PauseController>();
        PS4ControllerCheck = GameObject.FindWithTag("PS4ControllerCheck").GetComponent<PS4ControllerCheck>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        restoreShieldText.GetComponent<Text>().color = selected;
        restoreShieldArrow.GetComponent<Image>().color = selected;
        restoreShieldPriceText.GetComponent<Text>().color = selected;
        upgradeShieldText.GetComponent<Text>().color = nonselected;
        upgradeShieldArrow.GetComponent<Image>().color = nonselected;
        upgradeAttackText.GetComponent<Text>().color = nonselected;
        upgradeAttackArrow.GetComponent<Image>().color = nonselected;
        upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
        upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
        upgradeFuelArrow.GetComponent<Image>().color = nonselected;
        upgradeFuelText.GetComponent<Text>().color = nonselected;
        upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
        restoreFuelText.GetComponent<Text>().color = nonselected;
        restoreFuelArrow.GetComponent<Image>().color = nonselected;
        restoreFuelPriceText.GetComponent<Text>().color = nonselected;

        counterUpgrade = timeBetweenUpgrade;
    }

    // Update is called once per frame
    void Update()
    {
        availablePixelBlood.GetComponent<Text>().text = gameSession.CounterPixelBlood.ToString();
        numbersShield.GetComponent<Text>().text = gameSession.CurrentHealthSpacePlayer + " / " + gameSession.MaxHealthSpacePlayer;
        numbersFuel.GetComponent<Text>().text = (int)gameSession.CurrentFuelSpacePlayer + " / " + gameSession.MaxFuelSpacePlayer;
        numberAttack.GetComponent<Text>().text = "Att. " + gameSession.LaserDamage;
        upgradeShieldPriceText.GetComponent<Text>().text = "(" + gameSession.UpgradeShieldPrice + "pxl/50)";
        restoreShieldPriceText.GetComponent<Text>().text = "(" + gameSession.RestoreShieldPrice + "pxl/50)";
        restoreFuelPriceText.GetComponent<Text>().text = "(" + gameSession.RestoreFuelPrice + "pxl/50)";
        upgradeAttackPriceText.GetComponent<Text>().text = "(" + gameSession.UpgradeAttackPrice + "pxl/5)";
        upgradeFuelPriceText.GetComponent<Text>().text = "(" + gameSession.UpgradeFuelPrice + "pxl/50)";

        if (pauseController.IsGamePaused())
        {
            counterUpgrade -= Time.deltaTime;
            counterMove += Time.deltaTime;

            if (PS4ControllerCheck.DiscreteMoveUp() && selectedButton == "attackUpgrade" && counterMove > 0.1f)
            {
                selectedButton = "fuelUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = selected;
                upgradeFuelText.GetComponent<Text>().color = selected;
                upgradeFuelPriceText.GetComponent<Text>().color = selected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveDown() &&
            (selectedButton == "restoreShield" || selectedButton == "shieldUpgrade" || selectedButton == "fuelUpgrade" || selectedButton == "restoreFuel")
                 && counterMove > 0.1f)
            {
                selectedButton = "attackUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = selected;
                upgradeAttackArrow.GetComponent<Image>().color = selected;
                upgradeAttackPriceText.GetComponent<Text>().color = selected;
                upgradeFuelArrow.GetComponent<Image>().color = nonselected;
                upgradeFuelText.GetComponent<Text>().color = nonselected;
                upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveRight() && selectedButton == "restoreShield" && counterMove > 0.1f)
            {
                selectedButton = "shieldUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = selected;
                upgradeShieldArrow.GetComponent<Image>().color = selected;
                upgradeShieldPriceText.GetComponent<Text>().color = selected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = nonselected;
                upgradeFuelText.GetComponent<Text>().color = nonselected;
                upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveLeft() && selectedButton == "shieldUpgrade" && counterMove > 0.1f)
            {
                selectedButton = "restoreShield";
                restoreShieldText.GetComponent<Text>().color = selected;
                restoreShieldArrow.GetComponent<Image>().color = selected;
                restoreShieldPriceText.GetComponent<Text>().color = selected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = nonselected;
                upgradeFuelText.GetComponent<Text>().color = nonselected;
                upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveRight() && selectedButton == "shieldUpgrade" && counterMove > 0.1f)
            {
                selectedButton = "fuelUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = selected;
                upgradeFuelText.GetComponent<Text>().color = selected;
                upgradeFuelPriceText.GetComponent<Text>().color = selected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveLeft() && selectedButton == "fuelUpgrade" && counterMove > 0.1f)
            {
                selectedButton = "shieldUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = selected;
                upgradeShieldArrow.GetComponent<Image>().color = selected;
                upgradeShieldPriceText.GetComponent<Text>().color = selected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = nonselected;
                upgradeFuelText.GetComponent<Text>().color = nonselected;
                upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveRight() && selectedButton == "fuelUpgrade" && counterMove > 0.1f)
            {
                selectedButton = "restoreFuel";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = nonselected;
                upgradeFuelText.GetComponent<Text>().color = nonselected;
                upgradeFuelPriceText.GetComponent<Text>().color = nonselected;
                restoreFuelText.GetComponent<Text>().color = selected;
                restoreFuelArrow.GetComponent<Image>().color = selected;
                restoreFuelPriceText.GetComponent<Text>().color = selected;

                counterMove = 0f;
            }
            if (PS4ControllerCheck.DiscreteMoveLeft() && selectedButton == "restoreFuel" && counterMove > 0.1f)
            {
                selectedButton = "fuelUpgrade";
                restoreShieldText.GetComponent<Text>().color = nonselected;
                restoreShieldArrow.GetComponent<Image>().color = nonselected;
                restoreShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeShieldText.GetComponent<Text>().color = nonselected;
                upgradeShieldArrow.GetComponent<Image>().color = nonselected;
                upgradeShieldPriceText.GetComponent<Text>().color = nonselected;
                upgradeAttackText.GetComponent<Text>().color = nonselected;
                upgradeAttackArrow.GetComponent<Image>().color = nonselected;
                upgradeAttackPriceText.GetComponent<Text>().color = nonselected;
                upgradeFuelArrow.GetComponent<Image>().color = selected;
                upgradeFuelText.GetComponent<Text>().color = selected;
                upgradeFuelPriceText.GetComponent<Text>().color = selected;
                restoreFuelText.GetComponent<Text>().color = nonselected;
                restoreFuelArrow.GetComponent<Image>().color = nonselected;
                restoreFuelPriceText.GetComponent<Text>().color = nonselected;

                counterMove = 0f;
            }


            if (selectedButton == "shieldUpgrade")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    upgradeShieldText.GetComponent<Text>().color = clicked;
                    upgradeShieldArrow.GetComponent<Image>().color = clicked;
                    upgradeShieldPriceText.GetComponent<Text>().color = clicked;
                    if (counterUpgrade < 0f && gameSession.CounterPixelBlood >= gameSession.UpgradeShieldPrice)
                    {
                        gameSession.CounterPixelBlood -= gameSession.UpgradeShieldPrice;
                        gameSession.MaxHealthSpacePlayer += 50;
                        counterUpgrade = timeBetweenUpgrade;
                        timeBetweenUpgrade -= 0.03f;
                        timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                    }
                }
                else
                {
                    upgradeShieldText.GetComponent<Text>().color = selected;
                    upgradeShieldArrow.GetComponent<Image>().color = selected;
                    upgradeShieldPriceText.GetComponent<Text>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "fuelUpgrade")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    upgradeFuelText.GetComponent<Text>().color = clicked;
                    upgradeFuelArrow.GetComponent<Image>().color = clicked;
                    upgradeFuelPriceText.GetComponent<Text>().color = clicked;
                    if (counterUpgrade < 0f && gameSession.CounterPixelBlood >= gameSession.UpgradeFuelPrice)
                    {
                        gameSession.CounterPixelBlood -= gameSession.UpgradeFuelPrice;
                        gameSession.MaxFuelSpacePlayer += 50;
                        counterUpgrade = timeBetweenUpgrade;
                        timeBetweenUpgrade -= 0.03f;
                        timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                    }
                }
                else
                {
                    upgradeFuelText.GetComponent<Text>().color = selected;
                    upgradeFuelArrow.GetComponent<Image>().color = selected;
                    upgradeFuelPriceText.GetComponent<Text>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "attackUpgrade")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    upgradeAttackText.GetComponent<Text>().color = clicked;
                    upgradeAttackArrow.GetComponent<Image>().color = clicked;
                    upgradeAttackPriceText.GetComponent<Text>().color = clicked;
                    if (counterUpgrade < 0f && gameSession.CounterPixelBlood >= gameSession.UpgradeAttackPrice)
                    {
                        gameSession.CounterPixelBlood -= gameSession.UpgradeAttackPrice;
                        gameSession.LaserDamage += 5;
                        counterUpgrade = timeBetweenUpgrade;
                        timeBetweenUpgrade -= 0.03f;
                        timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                    }
                }
                else
                {
                    upgradeAttackText.GetComponent<Text>().color = selected;
                    upgradeAttackArrow.GetComponent<Image>().color = selected;
                    upgradeAttackPriceText.GetComponent<Text>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "restoreShield")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    if (gameSession.CurrentHealthSpacePlayer != gameSession.MaxHealthSpacePlayer)
                    {
                        restoreShieldText.GetComponent<Text>().color = clicked;
                        restoreShieldArrow.GetComponent<Image>().color = clicked;
                        restoreShieldPriceText.GetComponent<Text>().color = clicked;
                        if (counterUpgrade < 0f && gameSession.CounterPixelBlood >= gameSession.RestoreShieldPrice)
                        {
                            gameSession.CounterPixelBlood -= gameSession.RestoreShieldPrice;
                            gameSession.CurrentHealthSpacePlayer += 50;
                            counterUpgrade = timeBetweenUpgrade;
                            timeBetweenUpgrade -= 0.03f;
                            timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.001f, 0.1f);
                        }
                    }
                }
                else
                {
                    restoreShieldText.GetComponent<Text>().color = selected;
                    restoreShieldArrow.GetComponent<Image>().color = selected;
                    restoreShieldPriceText.GetComponent<Text>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }
            else if (selectedButton == "restoreFuel")
            {
                if (PS4ControllerCheck.ContinuousXPress())
                {
                    if ((int)gameSession.CurrentFuelSpacePlayer != (int)gameSession.MaxFuelSpacePlayer)
                    {
                        restoreFuelText.GetComponent<Text>().color = clicked;
                        restoreFuelArrow.GetComponent<Image>().color = clicked;
                        restoreFuelPriceText.GetComponent<Text>().color = clicked;
                        if (counterUpgrade < 0f && gameSession.CounterPixelBlood >= gameSession.RestoreFuelPrice)
                        {
                            gameSession.CounterPixelBlood -= gameSession.RestoreFuelPrice;
                            gameSession.CurrentFuelSpacePlayer += 50;
                            counterUpgrade = timeBetweenUpgrade;
                            timeBetweenUpgrade -= 0.03f;
                            timeBetweenUpgrade = Mathf.Clamp(timeBetweenUpgrade, 0.01f, 0.1f);
                        }
                    }
                }
                else
                {
                    restoreFuelText.GetComponent<Text>().color = selected;
                    restoreFuelArrow.GetComponent<Image>().color = selected;
                    restoreFuelPriceText.GetComponent<Text>().color = selected;
                    timeBetweenUpgrade = 0.1f;
                }
            }

        }
    }

}
