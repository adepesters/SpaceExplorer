using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileVaniaDoubleMirror : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    PlayerTileVania player;
    Vector3 pos;

    bool shouldMerge;
    bool isJumping;

    public SpriteRenderer Spriterenderer { get => spriterenderer; set => spriterenderer = value; }
    public bool ShouldMerge { get => shouldMerge; set => shouldMerge = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }

    // Start is called before the first frame update
    void Start()
    {
        Spriterenderer = GetComponentInChildren<SpriteRenderer>();
        Spriterenderer.enabled = false;

        player = FindObjectOfType<PlayerTileVania>();
    }

    private void OnTriggerStay2D(Collider2D collision)
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
        if (!shouldMerge && !IsJumping)
        {
            Pos = new Vector3(player.transform.position.x, -0.8f, 5.03f);
        }
        else if (!shouldMerge && IsJumping)
        {
            Pos = new Vector3(player.transform.position.x, player.transform.position.y + 1.3f, 5.03f);
        }
        else if (shouldMerge)
        {
            Pos = player.transform.position;
        }
        transform.position = Pos;
        Spriterenderer.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
        transform.localScale = player.transform.localScale;
    }

}
