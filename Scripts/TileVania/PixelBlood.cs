using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBlood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float forceX = UnityEngine.Random.Range(-10f, 10f);
        float forceY = UnityEngine.Random.Range(100f, 200f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
