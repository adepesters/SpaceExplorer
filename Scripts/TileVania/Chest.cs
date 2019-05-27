using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] int chestID;
    [SerializeField] bool isOpen = false;
    [SerializeField] int planetID;

    GameSession gameSession;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ActionTrigger>().MyDelegate1 = OpenChest;
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        isOpen = gameSession.OpenChests[planetID, chestID];
        if (isOpen)
        {
            GetComponentInParent<SpriteRenderer>().color = Color.red;
            GetComponent<ActionTrigger>().CanAppear = false;
            GetComponent<ActionTrigger>().DisableActionBox();
        }
    }

    void Update()
    {
        isOpen = gameSession.OpenChests[planetID, chestID];

        if (isOpen)
        {
            GetComponentInParent<SpriteRenderer>().color = Color.red;
            GetComponent<ActionTrigger>().CanAppear = false;
            GetComponent<ActionTrigger>().DisableActionBox();
        }
    }

    void OpenChest()
    {
        GetComponent<Chest>().IsOpen = true;
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        GetComponent<ActionTrigger>().CanAppear = false;
        GetComponent<ActionTrigger>().DisableActionBox();
        gameSession.OpenChests[1, chestID] = true;
    }
}
