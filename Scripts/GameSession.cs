using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameSession : MonoBehaviour
{
    [SerializeField] public int numberOfBronzeStars;
    [SerializeField] public int numberOfSilverStars;
    [SerializeField] private int numberOfGoldStars;

    public int counterStarBronze = 0;
    public int counterStarSilver = 0;
    public int counterStarGold = 0;

    int counterPixelBlood = 2000;

    // price for upgrade for planet player
    int healPrice = 2;
    int upgradeHealthPrice = 15;
    int upgradeStrengthPrice = 20;

    Player player;

    public int CounterPixelBlood { get => counterPixelBlood; set => counterPixelBlood = value; }
    public string SceneType { get => sceneType; set => sceneType = value; }
    public bool[,] OpenChests { get => openChests; set => openChests = value; }
    public bool[] HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }
    public bool[] HasBeenCompleted { get => hasBeenCompleted; set => hasBeenCompleted = value; }
    public int CurrentPlanetID { get => currentPlanetID; set => currentPlanetID = value; }
    public Vector3 PositionSpacePlayer { get => positionSpacePlayer; set => positionSpacePlayer = value; }
    public float CurrentFuelSpacePlayer { get => currentFuelSpacePlayer; set => currentFuelSpacePlayer = value; }
    public float MaxFuelSpacePlayer { get => maxFuelSpacePlayer; set => maxFuelSpacePlayer = value; }
    public int CurrentHealthSpacePlayer { get => currentHealthSpacePlayer; set => currentHealthSpacePlayer = value; }
    public int MaxHealthSpacePlayer { get => maxHealthSpacePlayer; set => maxHealthSpacePlayer = value; }
    public Vector3 PositionPointer { get => positionPointer; set => positionPointer = value; }
    public bool[] IsCleaned { get => isCleaned; set => isCleaned = value; }
    public int CurrentHealthPlanetPlayer { get => currentHealthPlanetPlayer; set => currentHealthPlanetPlayer = value; }
    public int MaxHealthPlanetPlayer { get => maxHealthPlanetPlayer; set => maxHealthPlanetPlayer = value; }
    public int SwordDamage { get => swordDamage; set => swordDamage = value; }
    public int NumberOfGoldStars { get => numberOfGoldStars; set => numberOfGoldStars = value; }
    public int HealPrice { get => healPrice; set => healPrice = value; }
    public int UpgradeHealthPrice { get => upgradeHealthPrice; set => upgradeHealthPrice = value; }
    public int UpgradeStrengthPrice { get => upgradeStrengthPrice; set => upgradeStrengthPrice = value; }
    public int CurrentLayerSpacePlayer { get => currentLayerSpacePlayer; set => currentLayerSpacePlayer = value; }

    [SerializeField] string sceneType = "space"; // "space" or "planet"

    // space player info to save/load
    Vector3 positionSpacePlayer;
    float currentFuelSpacePlayer;
    float maxFuelSpacePlayer;
    int currentHealthSpacePlayer;
    int maxHealthSpacePlayer;

    // planet player info to save/load
    int currentHealthPlanetPlayer;
    int maxHealthPlanetPlayer;
    int swordDamage;

    // planets info to save/load
    bool[,] openChests = new bool[50, 50];
    [SerializeField] bool[] hasBeenDiscovered = new bool[50];
    [SerializeField] bool[] hasBeenCompleted = new bool[50];
    [SerializeField] bool[] isCleaned = new bool[100];

    int currentPlanetID;

    Vector3 positionPointer;

    int currentLayerSpacePlayer = 1;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Space")
        {
            sceneType = "space";
        }
        else if (SceneManager.GetActiveScene().name.Contains("Planet"))
        {
            sceneType = "planet";
            string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
            currentPlanetID = int.Parse(numbersOnly);
        }

        if (sceneType == "space")
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();

            // initializes planets data
            for (int planet = 0; planet < OpenChests.GetLength(0); planet++)
            {
                HasBeenDiscovered[planet] = false;
                HasBeenCompleted[planet] = false;

                for (int chest = 0; chest < OpenChests.GetLength(1); chest++)
                {
                    OpenChests[planet, chest] = false;
                }
            }
            // initializes space player data
            //Vector3 initialPos = GameObject.Find("Planet 1").gameObject.transform.position;
            //Vector3 initialPos = GameObject.Find("Zone 3").gameObject.transform.position;
            Vector3 initialPos = player.transform.position;
            positionSpacePlayer = new Vector3(initialPos.x, initialPos.y, 0);
            maxFuelSpacePlayer = 20000f;
            currentFuelSpacePlayer = maxFuelSpacePlayer;
            maxHealthSpacePlayer = 6000;
            currentHealthSpacePlayer = maxHealthSpacePlayer;

            // initializes pointer data
            positionPointer = new Vector3(initialPos.x, initialPos.y, 0);

            // initializes spawning enemy areas
            for (int spawningArea = 0; spawningArea < IsCleaned.Length; spawningArea++)
            {
                IsCleaned[spawningArea] = false;
            }
        }

        // initializes planet player data
        maxHealthPlanetPlayer = 100;
        currentHealthPlanetPlayer = maxHealthPlanetPlayer;
        SwordDamage = 15;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    if (sceneType == "space")
    //    {
    //        for (int i = 0; i < CounterPixelBlood.Length - 1; i++)
    //        {
    //            CounterPixelBlood[i] = 0;
    //        }
    //    }
    //}

    void Update()
    {
        if (sceneType == "space")
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<Player>();
            }
        }
    }
}
