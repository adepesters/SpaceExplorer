﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapIcon : MonoBehaviour
{
    Player player;

    float discoveryThresholdDist = 25f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.parent.transform.position) < discoveryThresholdDist)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}