using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public int score;
    [SerializeField] Text scoreText;
    [SerializeField] public Text healthText;
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

    string sceneType = "space"; // "space" or "planet"

    bool[,] openChests = new bool[1, 2];

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

        if (sceneType == "space")
        {
            for (int planet = 0; planet < OpenChests.GetLength(0); planet++)
            {
                for (int chest = 0; chest < OpenChests.GetLength(0); chest++)
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

            //scoreText.text = score.ToString("D6");
            healthText.text = player.GetHealthPlayer().ToString();

            for (int i = 0; i < CounterPixelBlood.Length - 1; i++)
            {
                CounterPixelBlood[i] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneType == "space")
        {
            //scoreText.text = score.ToString("D6");
            if (player != null) // because when the player dies its game object gets destructed, which returns 
                                // a bug when we try to access a FindObjectOfType<Player>
            {
                healthText.text = player.GetHealthPlayer().ToString();
            }
            else
            {
                FindObjectOfType<GameSession>().healthText.text = "000";
            }
        }

        //for (int i = 0; i < CounterPixelBlood.Length - 1; i++)
        //{
        //    Debug.Log(i + ": " + CounterPixelBlood[i]);
        //}

    }
}
