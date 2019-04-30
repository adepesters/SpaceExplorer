using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRadarActivator : MonoBehaviour
{
    private bool hasBeenDiscovered;

    public bool HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }

    Player player;

    float discoveryThresholdDist = 15f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        HasBeenDiscovered = false;
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
