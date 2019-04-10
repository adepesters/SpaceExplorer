using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    void Start()
    {
        FindObjectOfType<Shield>().gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        FindObjectOfType<Shield>().gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        Vector2 pos = new Vector2(FindObjectOfType<Player>().transform.position.x, FindObjectOfType<Player>().transform.position.y);
        transform.position = pos;
    }


}
