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

    bool playerInPortal;

    public SpriteRenderer Spriterenderer { get => spriterenderer; set => spriterenderer = value; }
    public bool ShouldMerge { get => shouldMerge; set => shouldMerge = value; }
    public Vector3 Pos { get => pos; set => pos = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public Rigidbody2D Myrigidbody { get => myrigidbody; set => myrigidbody = value; }
    public Transform EntryFrontLayer { get => entryFrontLayer; set => entryFrontLayer = value; }
    public Transform EntryBackLayer { get => entryBackLayer; set => entryBackLayer = value; }
    public bool IsCrossing { get => isCrossing; set => isCrossing = value; }
    public bool PlayerInPortal { get => playerInPortal; set => playerInPortal = value; }

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
        if (collision.gameObject.name.Contains("Bridge") && playerInPortal)
        {
            Spriterenderer.enabled = true;
            GetComponentInChildren<ErasePixels>().Portal = collision.gameObject.transform.parent.transform.GetChild(0).gameObject;
        }

        if (collision.gameObject.name.Contains("Bridge") && !playerInPortal)
        {
            Spriterenderer.enabled = false;
            GetComponentInChildren<ErasePixels>().Portal = collision.gameObject.transform.parent.transform.GetChild(0).gameObject;
        }

        if (collision.gameObject.name.Contains("StartUpdatingPixels") && playerInPortal)
        {
            GetComponentInChildren<ErasePixels>().UpdateColors = true;
            Spriterenderer.enabled = true;
        }

        if (collision.gameObject.name.Contains("StartUpdatingPixels") && !playerInPortal)
        {
            GetComponentInChildren<ErasePixels>().UpdateColors = true;
            Spriterenderer.enabled = false;
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
            if (Mathf.Abs(entryFrontLayer.position.z - player.transform.position.z) < Mathf.Abs(entryBackLayer.position.z - player.transform.position.z)) // player is on layer of front entry
            {
                Pos = new Vector3(player.transform.position.x,
                player.transform.position.y + (entryBackLayer.position.y - entryFrontLayer.position.y),
                    entryBackLayer.position.z + 0.03f);
            }
            else
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
