﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicRock : MonoBehaviour
{
    Vector3 target;

    public Vector3 Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = BallisticVel();
    }

    private Vector3 BallisticVel()
    {
        var dir = Target - transform.position; // get target direction
        var h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        Debug.Log(dist);
        dir.y = dist;  // set elevation to 45 degrees
        dist += h;  // correct for different heights
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
        return vel * dir.normalized;  // returns Vector3 velocity
    }

}
