using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    bool treeIsTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
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
            if (!FindObjectOfType<Feet>().CurrentSurface.name.Contains("Ground"))
            {
                if (!(FindObjectOfType<PlayerTileVania>().GetComponent<Rigidbody2D>().velocity.y > 0f))
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
