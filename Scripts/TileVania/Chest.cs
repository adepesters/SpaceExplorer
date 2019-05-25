using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] int chestID;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ActionTrigger>().MyDelegate1 = ChangeColor;
    }

    void ChangeColor()
    {
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        GetComponent<ActionTrigger>().CanAppear = false;
        GetComponent<ActionTrigger>().DisableActionBox();
    }
}
