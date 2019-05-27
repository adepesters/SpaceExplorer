using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapIcon : MonoBehaviour
{
    bool hasBeenDisovered = false;

    Player player;

    int planetID;

    float discoveryThresholdDist = 25f;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GetComponent<SpriteRenderer>().enabled = false;
        planetID = GetComponentInParent<Planet>().PlanetID;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameSession.HasBeenDiscovered[planetID])
        {
            if (Vector2.Distance(player.transform.position, transform.parent.transform.position) < discoveryThresholdDist)
            {
                hasBeenDisovered = true;
                gameSession.HasBeenDiscovered[planetID] = true;
            }
        }
        else
        {
            hasBeenDisovered = true;
        }
        if (hasBeenDisovered)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

}
