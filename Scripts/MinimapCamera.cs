﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(FindObjectOfType<Player>().transform.position.x, FindObjectOfType<Player>().transform.position.y, -5);
    }
}
