using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    bool treeIsTrigger = true;

    PlayerTileVania player;
    Feet feet;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
        feet = FindObjectOfType<Feet>();

        if (treeIsTrigger)
        {
            GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (treeIsTrigger)
        {
            GetComponent<PolygonCollider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Feet"))
        {
            if (!feet.CurrentSurface.name.Contains("Ground"))
            {
                if (!(player.GetComponent<Rigidbody2D>().velocity.y > 0f))
                {
                    treeIsTrigger = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Feet"))
        {
            treeIsTrigger = true;
        }
    }

}
