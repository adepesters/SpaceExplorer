using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLayer : MonoBehaviour
{
    int layer;

    public int Layer { get => layer; set => layer = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.z < 20 && transform.position.z > -10)
        {
            layer = 1;
        }
        if (transform.position.z < 40 && transform.position.z > 20)
        {
            layer = 2;
        }
        if (transform.position.z < 70 && transform.position.z > 40)
        {
            layer = 3;
        }
    }

    void Update()
    {
        if (transform.position.z < 20 && transform.position.z > -10)
        {
            layer = 1;
        }
        if (transform.position.z < 40 && transform.position.z > 20)
        {
            layer = 2;
        }
        if (transform.position.z < 70 && transform.position.z > 40)
        {
            layer = 3;
        }
    }

}
