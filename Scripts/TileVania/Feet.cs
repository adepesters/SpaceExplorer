using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    PlayerTileVania player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        player.FeetAreOnSomething = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.FeetAreOnSomething = false;
    }
}
