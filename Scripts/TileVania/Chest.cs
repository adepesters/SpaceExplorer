using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    [SerializeField] int chestID;
    [SerializeField] bool isOpen = false;
    int planetID;

    GameSession gameSession;
    PlayerTileVania player;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);

        player = FindObjectOfType<PlayerTileVania>();

        GetComponent<ActionTrigger>().MyDelegate1 = OpenChest;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        isOpen = gameSession.OpenChests[planetID, chestID];
        if (isOpen)
        {
            GetComponentInParent<SpriteRenderer>().color = Color.red;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<ActionTrigger>().CanAppear = false;
            GetComponent<ActionTrigger>().DisableActionBox();
            player.XisActionTrigger1 = false;
        }
    }

    void OpenChest()
    {
        isOpen = true;
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<ActionTrigger>().CanAppear = false;
        GetComponent<ActionTrigger>().DisableActionBox();
        gameSession.OpenChests[1, chestID] = true;
        player.XisActionTrigger1 = false;
    }
}
