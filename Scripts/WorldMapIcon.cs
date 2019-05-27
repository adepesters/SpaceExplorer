using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapIcon : MonoBehaviour
{
    bool hasBeenDisovered = false;

    Player player;

    [SerializeField] int planetID;

    float discoveryThresholdDist = 25f;

    public bool HasBeenDiscovered { get => hasBeenDisovered; set => hasBeenDisovered = value; }

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GetComponent<SpriteRenderer>().enabled = false;
        hasBeenDisovered = gameSession.HasBeenDiscovered[planetID];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.parent.transform.position) < discoveryThresholdDist)
        {
            hasBeenDisovered = true;
        }
        if (hasBeenDisovered)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
