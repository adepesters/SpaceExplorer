using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileVaniaDoubleMirror : MonoBehaviour
{
    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            renderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            renderer.enabled = false;
        }
    }
}
