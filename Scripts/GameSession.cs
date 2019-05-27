using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] public int numberOfBronzeStars;
    [SerializeField] public int numberOfSilverStars;
    [SerializeField] public int numberOfGoldStars;

    public int counterStarBronze = 0;
    public int counterStarSilver = 0;
    public int counterStarGold = 0;

    int[] counterPixelBlood = new int[8];

    Player player;

    public int[] CounterPixelBlood { get => counterPixelBlood; set => counterPixelBlood = value; }
    public string SceneType { get => sceneType; set => sceneType = value; }
    public bool[,] OpenChests { get => openChests; set => openChests = value; }
    public bool[] HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }
    public bool[] HasBeenCompleted { get => hasBeenCompleted; set => hasBeenCompleted = value; }
    public int CurrentPlanetID { get => currentPlanetID; set => currentPlanetID = value; }

    [SerializeField] string sceneType = "space"; // "space" or "planet"

    // planet related info to access
    bool[,] openChests = new bool[50, 50];
    [SerializeField] bool[] hasBeenDiscovered = new bool[50];
    [SerializeField] bool[] hasBeenCompleted = new bool[50];

    int currentPlanetID;

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
        }

        if (sceneType == "space")
        {
            for (int planet = 0; planet < OpenChests.GetLength(0); planet++)
            {
                HasBeenDiscovered[planet] = false;
                HasBeenCompleted[planet] = false;

                for (int chest = 0; chest < OpenChests.GetLength(1); chest++)
                {
                    OpenChests[planet, chest] = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sceneType == "space")
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();

            for (int i = 0; i < CounterPixelBlood.Length - 1; i++)
            {
                CounterPixelBlood[i] = 0;
            }
        }
    }

}
