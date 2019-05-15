using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer1 : MonoBehaviour
{
    PolygonCollider2D mycollider2D;
    // Start is called before the first frame update
    void Start()
    {
        mycollider2D = GetComponent<PolygonCollider2D>();
        mycollider2D.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
