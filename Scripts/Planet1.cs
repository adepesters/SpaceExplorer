﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet1 : MonoBehaviour
{
    bool hasBeenDiscovered = false;
    bool hasBeenCompleted = false;
    bool[] openChests;
    YellowRadarActivator yellowRadarActivator;

    public bool HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }
    public bool HasBeenCompleted { get => hasBeenCompleted; set => hasBeenCompleted = value; }
    public bool[] OpenChests { get => openChests; set => openChests = value; }

    // Start is called before the first frame update
    void Start()
    {
        OpenChests = new bool[2];
        for (int i = 0; i < OpenChests.Length; i++)
        {
            OpenChests[i] = false;
        }

        yellowRadarActivator = GetComponent<YellowRadarActivator>();
        HasBeenDiscovered = yellowRadarActivator.HasBeenDiscovered;
    }

    // Update is called once per frame
    void Update()
    {
        HasBeenDiscovered = yellowRadarActivator.HasBeenDiscovered;
    }
}