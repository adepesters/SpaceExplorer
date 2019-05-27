using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRadarActivator : MonoBehaviour
{
    bool hasBeenDiscovered;

    public bool HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }

    Player player;

    float discoveryThresholdDist = 15f;

    [SerializeField] int planetID;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        HasBeenDiscovered = gameSession.HasBeenDiscovered[planetID];

        //HasBeenDiscovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < discoveryThresholdDist)
        {
            hasBeenDiscovered = true;
        }
    }


}
