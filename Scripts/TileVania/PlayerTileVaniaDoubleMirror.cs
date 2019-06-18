using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileVaniaDoubleMirror : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    PlayerTileVania player;
    Vector3 pos;

    Transform entryFrontLayer;
    Transform entryBackLayer;

    bool isCrossing = false;

    bool shouldMerge;
    bool isJumping;
    Rigidbody2D myrigidbody;

    public SpriteRenderer Spriterenderer { get => spriterenderer; set => spriterenderer = value; }
    public bool ShouldMerge { get => shouldMerge; set => shouldMerge = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public Rigidbody2D Myrigidbody { get => myrigidbody; set => myrigidbody = value; }
    public Transform EntryFrontLayer { get => entryFrontLayer; set => entryFrontLayer = value; }
    public Transform EntryBackLayer { get => entryBackLayer; set => entryBackLayer = value; }
    public bool IsCrossing { get => isCrossing; set => isCrossing = value; }

    // Start is called before the first frame update
    void Start()
    {
        Spriterenderer = GetComponentInChildren<SpriteRenderer>();
        Spriterenderer.enabled = false;

        myrigidbody = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<PlayerTileVania>();

        entryFrontLayer = player.transform;
        entryBackLayer = player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bridge"))
        {
            Spriterenderer.enabled = true;
        }
        if (collision.gameObject.name.Contains("StartUpdatingPixels"))
        {
            GetComponentInChildren<ErasePixels>().UpdateColors = true;
            Spriterenderer.enabled = true;
        }
        if (collision.gameObject.name.Contains("StopUpdatingPixels"))
        {
            Spriterenderer.enabled = false;
            GetComponentInChildren<ErasePixels>().UpdateColors = false;
        }
    }

    void Update()
    {
        if (!IsCrossing)
        {
            if (player.CurrentLayer == 1)
            {
                Pos = new Vector3(player.transform.position.x,
                player.transform.position.y + (entryBackLayer.position.y - entryFrontLayer.position.y),
                    entryBackLayer.position.z + 0.03f);
            }
            else if (player.CurrentLayer == 2)
            {
                Pos = new Vector3(player.transform.position.x,
                player.transform.position.y + (entryFrontLayer.position.y - entryBackLayer.position.y),
                    entryFrontLayer.position.z + 0.03f);
            }
        }

        transform.position = Pos;
        Spriterenderer.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
        transform.localScale = player.transform.localScale;
    }

}
