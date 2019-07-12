using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRadarActivator : MonoBehaviour
{
    Player player;

    float discoveryThresholdDist = 15f;

    int planetID;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        planetID = GetComponent<Planet>().PlanetID;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameSession.HasBeenDiscovered[planetID])
        {
            if (Vector3.Distance(player.transform.position, transform.position) < discoveryThresholdDist)
            {
                gameSession.HasBeenDiscovered[planetID] = true;
            }
        }
    }

}
