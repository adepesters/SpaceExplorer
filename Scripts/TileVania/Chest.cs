using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] int chestID;
    [SerializeField] bool isOpen = false;

    GameSession gameSession;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ActionTrigger>().MyDelegate1 = OpenChest;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    void OpenChest()
    {
        GetComponent<Chest>().IsOpen = true;
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        GetComponent<ActionTrigger>().CanAppear = false;
        GetComponent<ActionTrigger>().DisableActionBox();
        gameSession.OpenChests[0, chestID] = true;
    }
}
