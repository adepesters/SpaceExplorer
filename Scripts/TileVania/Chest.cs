using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ActionTrigger>().MyDelegate1 = ChangeColor;
    }

    void ChangeColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
