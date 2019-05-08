using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        FindObjectOfType<Shield>().gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        FindObjectOfType<Shield>().gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        Vector2 pos = new Vector2(player.transform.position.x, player.transform.position.y);
        transform.position = pos;
    }


}
