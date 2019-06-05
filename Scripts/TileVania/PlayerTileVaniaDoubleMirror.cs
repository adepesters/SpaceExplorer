﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileVaniaDoubleMirror : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    PlayerTileVania player;
    Vector3 pos;

    bool shouldMerge;
    bool isJumping;
    Rigidbody2D myrigidbody;

    public SpriteRenderer Spriterenderer { get => spriterenderer; set => spriterenderer = value; }
    public bool ShouldMerge { get => shouldMerge; set => shouldMerge = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public Rigidbody2D Myrigidbody { get => myrigidbody; set => myrigidbody = value; }

    // Start is called before the first frame update
    void Start()
    {
        Spriterenderer = GetComponentInChildren<SpriteRenderer>();
        Spriterenderer.enabled = false;

        myrigidbody = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<PlayerTileVania>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            Spriterenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            Spriterenderer.enabled = false;
        }
    }

    void Update()
    {
        if (!IsJumping)
        {
            if (player.CurrentLayer == 1)
            {
                Pos = new Vector3(player.transform.position.x, -0.4f, 5.03f);
            }
            else if (player.CurrentLayer == 2)
            {
                Pos = new Vector3(player.transform.position.x, -1.4f, 0.03f);
            }
        }
        else
        {
            if (player.CurrentLayer == 1)
            {
                Pos = new Vector3(player.transform.position.x, player.transform.position.y + 1.3f, 5.03f);
            }
            else if (player.CurrentLayer == 2)
            {
                Pos = new Vector3(player.transform.position.x, player.transform.position.y + -1.0f, 0.03f);
            }
        }
        transform.position = Pos;
        Spriterenderer.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
        transform.localScale = player.transform.localScale;
    }

}
