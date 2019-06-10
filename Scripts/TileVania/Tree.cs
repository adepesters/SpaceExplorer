using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    PlayerTileVania player;
    Feet feet;
    ExtendedLegs extendedLegs;

    bool feetTouching;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
        feet = FindObjectOfType<Feet>();
        extendedLegs = FindObjectOfType<ExtendedLegs>();

        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsJumping && player.GetComponent<Rigidbody2D>().velocity.y < 0f && feetTouching)
        {
            GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Feet"))
        {
            feetTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Extended Legs"))
        {
            feetTouching = false;
            GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

}
