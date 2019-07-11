using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.position = pos;
    }


}
