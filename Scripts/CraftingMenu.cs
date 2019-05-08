using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{

    [SerializeField] Text[] numberOfBonuses;
    [SerializeField] GameObject upgradeNotificationPrefab;
    GameObject newUpgradeNotificationPrefab;

    // Upgrade 1
    [Header("Upgrade 1")]
    [SerializeField] Text oldUpgrade1ValueText;
    [SerializeField] Text newUpgrade1ValueText;
    [SerializeField] Text costBronzeUpgrade1Text;
    [SerializeField] Text costSilverUpgrade1Text;
    [SerializeField] Text costGoldUpgrade1Text;
    [SerializeField] Image validUpgradeImage;

    List<int> upgrade1Values = new List<int> { 100, 200, 300, 400 };
    int indexUpgrade1 = 0;
    List<int> numberOfBronzeStarsForUpgrade1 = new List<int> { 10, 20, 30 };
    List<int> numberOfSilverStarsForUpgrade1 = new List<int> { 0, 1, 3 };
    List<int> numberOfGoldStarsForUpgrade1 = new List<int> { 0, 0, 1 };
    bool upgrade1NotificationHasPlayed = false;

    GameSession gameSession;
    PauseMenuController pauseMenuController;
    PauseController pauseController;
    Player player;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        pauseMenuController = FindObjectOfType<PauseMenuController>();
        pauseController = FindObjectOfType<PauseController>();
        player = FindObjectOfType<Player>();

        numberOfBonuses[0].text = gameSession.counterStarBronze.ToString();
        numberOfBonuses[1].text = gameSession.counterStarSilver.ToString();
        numberOfBonuses[2].text = gameSession.counterStarGold.ToString();

        gameObject.GetComponent<Canvas>().enabled = false;

        // upgrade 1
        oldUpgrade1ValueText.text = upgrade1Values[indexUpgrade1].ToString();
        newUpgrade1ValueText.text = upgrade1Values[indexUpgrade1 + 1].ToString();
        costBronzeUpgrade1Text.text = numberOfBronzeStarsForUpgrade1[indexUpgrade1].ToString();
        costSilverUpgrade1Text.text = numberOfSilverStarsForUpgrade1[indexUpgrade1].ToString();
        costGoldUpgrade1Text.text = numberOfGoldStarsForUpgrade1[indexUpgrade1].ToString();
        validUpgradeImage.GetComponent<Image>().color = new Color(1, 0, 0, 1);
    }

    void Update()
    {
        if (pauseController.IsGamePaused() && pauseMenuController.GetMenuPage() == "crafting")
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            numberOfBonuses[0].text = gameSession.counterStarBronze.ToString();
            numberOfBonuses[1].text = gameSession.counterStarSilver.ToString();
            numberOfBonuses[2].text = gameSession.counterStarGold.ToString();

            if (IsUpgrade1Available())
            {
                validUpgradeImage.GetComponent<Image>().color = new Color(0.3f, 1, 0.3f, 1);
            }
            else
            {
                validUpgradeImage.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            }

            if (newUpgradeNotificationPrefab == null)
            {
                if (IsUpgrade1Available() && upgrade1NotificationHasPlayed == false)
                {
                    StartCoroutine(InstantiateUpgradeNotification());
                    upgrade1NotificationHasPlayed = true;
                }
            }
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }

    }

    IEnumerator InstantiateUpgradeNotification()
    {
        newUpgradeNotificationPrefab = Instantiate(upgradeNotificationPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(7);
        Destroy(newUpgradeNotificationPrefab);
    }

    private bool IsUpgrade1Available()
    {
        if (gameSession.counterStarBronze >= numberOfBronzeStarsForUpgrade1[indexUpgrade1] &&
        gameSession.counterStarSilver >= numberOfSilverStarsForUpgrade1[indexUpgrade1] &&
            gameSession.counterStarGold >= numberOfGoldStarsForUpgrade1[indexUpgrade1])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Upgrade1()
    {
        if (IsUpgrade1Available())
        {
            indexUpgrade1++;
            indexUpgrade1 = Mathf.Clamp(indexUpgrade1, 0, upgrade1Values.Count - 1);
            player.SetLaserDamage(upgrade1Values[indexUpgrade1]);
            if (indexUpgrade1 == upgrade1Values.Count - 1)
            {
                oldUpgrade1ValueText.text = upgrade1Values[indexUpgrade1].ToString();
                newUpgrade1ValueText.text = "MAX";
                costBronzeUpgrade1Text.text = "-";
                costSilverUpgrade1Text.text = "-";
                costGoldUpgrade1Text.text = "-";
            }
            else
            {
                oldUpgrade1ValueText.text = upgrade1Values[indexUpgrade1].ToString();
                newUpgrade1ValueText.text = upgrade1Values[indexUpgrade1 + 1].ToString();
                costBronzeUpgrade1Text.text = numberOfBronzeStarsForUpgrade1[indexUpgrade1].ToString();
                costSilverUpgrade1Text.text = numberOfSilverStarsForUpgrade1[indexUpgrade1].ToString();
                costGoldUpgrade1Text.text = numberOfGoldStarsForUpgrade1[indexUpgrade1].ToString();
            }
            gameSession.counterStarBronze -= numberOfBronzeStarsForUpgrade1[indexUpgrade1 - 1];
            gameSession.counterStarSilver -= numberOfSilverStarsForUpgrade1[indexUpgrade1 - 1];
            gameSession.counterStarGold -= numberOfGoldStarsForUpgrade1[indexUpgrade1 - 1];
            upgrade1NotificationHasPlayed = false;
        }
    }

}
